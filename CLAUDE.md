# CLAUDE.md

Guidance for working in this repository. The full specification lives in `Assignment.docx`; this file is the distilled, actionable summary. **When in doubt, the assignment document is the source of truth.**

## What this is

A **Library Management System (LMS)** — a web app where a **Librarian** manages books/authors/members and **Members** browse, borrow, and return books. Overdue books trigger asynchronous alerts. This is a graduate-trainee assignment (2-week duration) graded on a code walkthrough; **every mandatory technology must be genuinely exercised in the running app**, not just referenced.

- **Backend:** .NET Core Web API, Clean Architecture (4 projects)
- **Frontend:** Angular 17+ (standalone components)
- **Data:** SQL Server (primary) + Redis (cache & token revocation)
- **Cloud (local emulators):** Azure Blob Storage (book covers) + Azure Service Bus (overdue alerts)

## Non-negotiable: Clean Architecture dependency rule

The backend is **exactly four projects**. Dependencies point **inward only**. A violation (an inner layer referencing an outer one) fails the entire Architecture review section regardless of working features.

| Layer | Project | Contains | Allowed packages |
|---|---|---|---|
| Domain (innermost) | `LMS.Domain` | Entities (`Book`, `Author`, `Member`, `BorrowRecord`), enums (`UserRole`), custom exceptions (`NotFoundException`, `BusinessRuleException`) | **None — pure C#** |
| Application | `LMS.Application` | Commands, Queries, DTOs, AutoMapper profiles, FluentValidation validators, **all service interfaces** | MediatR, AutoMapper, FluentValidation. **Depends on Domain only** |
| Infrastructure | `LMS.Infrastructure` | EF Core `DbContext` + `IEntityTypeConfiguration` classes, repository impls, Redis/Blob/Service Bus services, hosted services, Serilog config | EF Core+SQL Server, Redis, Azure.Storage.Blobs, Azure.Messaging.ServiceBus, Serilog |
| API (outermost) | `LMS.API` | Controllers, `ExceptionHandlingMiddleware`, JWT config, Swagger, **all DI registrations in `Program.cs`** | ASP.NET Core, JWT Bearer, Swagger, Serilog |

**Hard constraints (verified at review):**
- `LMS.Domain` has **zero** NuGet references; entities are plain C# classes (no DataAnnotations).
- `LMS.Application` references `LMS.Domain` only — no EF Core, Azure SDK, or Redis packages.
- All service interfaces are **defined in Application**, **implemented in Infrastructure**.
- Controllers call **`_mediator.Send()` only** — never instantiate a repository/service or reference Infrastructure types.
- Interface→concrete DI bindings live **only** in `Program.cs` (LMS.API).

## CQRS / MediatR

Every feature is a **Command** or **Query** with a dedicated handler. A `ValidationPipelineBehaviour` is registered globally so FluentValidation runs automatically before every handler (throws `ValidationException` → mapped to 400 by middleware). All response data flows through **AutoMapper** — no manual property copying.

## Required service interfaces (Application layer)

Defined in `LMS.Application`, implemented in `LMS.Infrastructure`:

- `IBookRepository` — GetByIdAsync, GetPagedAsync (filter authorId, keyword), CreateAsync, UpdateAsync, SoftDeleteAsync
- `IAuthorRepository` — GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, SoftDeleteAsync
- `IMemberRepository` — GetByIdAsync, GetByEmailAsync, GetAllAsync, CreateAsync, UpdateAsync, DeactivateAsync
- `IBorrowRepository` — CreateAsync, GetActiveByMemberAsync, GetByIdAsync, MarkReturnedAsync, GetOverdueAsync
- `IUnitOfWork` — SaveChangesAsync (wraps EF Core)
- `ICoverImageService` — UploadAsync(bookId, stream, contentType)→URL; DeleteAsync(blobUrl)
- `IOverdueAlertPublisher` — PublishAsync(bookId, bookTitle, memberEmail, dueDate)
- `ICacheService` — GetAsync<T>(key), SetAsync<T>(key, value, ttl), RemoveAsync(key), RemoveByPrefixAsync(prefix)
- `ITokenService` — GenerateAccessToken(member), GenerateRefreshToken(), IsRefreshTokenRevoked(token)
- `IOverdueCheckerService` — CheckAndPublishOverdueAsync() (driven by the hosted timer)

## Core business rules

- Only authenticated users access the app. Unauthenticated requests (except register/login) → **401**.
- Only a **Librarian** can add/update/delete books & authors and manage members. Librarians **cannot borrow**.
- A book: title, description, **ISBN (unique, exactly 13 digits)**, publication year, total copies, one author, optional cover.
- Cover image uploaded to Blob path `covers/{bookId}/{guid}_{filename}`; DB stores the returned URL.
- Borrow allowed only if an available copy exists (`TotalCopies − active borrows > 0`).
- On borrow: create `BorrowRecord` with `BorrowedAt=now`, `DueDate=now+14 days`; decrement available count.
- On return: set `ReturnedAt=now`; increment available count.
- A member may hold **at most 3 active borrows** simultaneously.
- Daily: find all records where `DueDate < UtcNow` and `ReturnedAt is null` → publish overdue alert to Service Bus; consumer logs a structured warning.
- All inputs validated server-side → **400** with field-level errors.
- All unhandled exceptions caught by **one** global middleware → structured JSON envelope. **Never expose stack traces.**

## Exception → HTTP status mapping (single `ExceptionHandlingMiddleware`, registered first)

| Exception | Status |
|---|---|
| `ValidationException` (FluentValidation) | 400 — list of `{ field, message }` |
| `NotFoundException` | 404 |
| `BusinessRuleException` (no copies, borrow limit, etc.) | **422** |
| `UnauthorizedAccessException` | 403 |
| anything else | 500 — `{ success:false, statusCode:500, error:"An unexpected error occurred.", traceId:"..." }` |

All exceptions logged at Error level via Serilog before the response is written.

## API surface (prefix `/api/v1`, Bearer auth except where noted)

**Auth (public):** `POST /auth/register`, `POST /auth/login`, `POST /auth/refresh` (checks Redis revocation), `POST /auth/logout` (authenticated; writes to Redis).

**Authors:** `GET /authors` , `GET /authors/{id}` [authenticated]; `POST /authors`, `PUT /authors/{id}`, `DELETE /authors/{id}` [Librarian].

**Books:** `GET /books?authorId=&q=&page=&pageSize=` , `GET /books/{id}` [authenticated, Redis-cached]; `POST /books`, `PUT /books/{id}` (invalidate cache), `DELETE /books/{id}` (delete blob + invalidate cache), `POST /books/{id}/cover` (multipart), `DELETE /books/{id}/cover` [Librarian].

**Borrows:** `POST /borrows` (body `{ bookId }`), `PATCH /borrows/{id}/return`, `GET /borrows/my` [Member]; `GET /borrows` [Librarian].

**Members:** `GET /members?q=&page=&pageSize=`, `GET /members/{id}`, `DELETE /members/{id}` [Librarian]; `GET /members/me` [Member].

## Auth details

- **Access token:** JWT, **30 min**, claims = userId, email, role. `[Authorize(Roles="Librarian")]` on librarian endpoints.
- **Refresh token:** **7 days**, stored in `RefreshTokens` table.
- **Refresh:** reject if present in Redis revocation set (`revoked:{token}`); else issue new access token + rotate refresh token.
- **Logout:** write refresh token to Redis `revoked:{token}` with TTL = remaining lifetime.
- **Register:** Member role only (Librarians are seeded). Password ≥ 8 chars, ≥1 uppercase, ≥1 digit; valid email; full name required ≤120 chars.

## Redis usage (two distinct purposes)

1. **Refresh-token revocation set** — checked on every `/auth/refresh`.
2. **Response cache:**
   - Book list: key `books:list:{authorId}:{keyword}:{page}:{pageSize}`, TTL **5 min**.
   - Book detail: key `books:detail:{bookId}`, TTL **10 min**.
   - Writes (book create/update/delete) invalidate via `RemoveByPrefixAsync("books:")`.
   - Log cache hit/miss at Debug: `Cache {Result} for key {CacheKey}`.

## Overdue alert flow (Service Bus + Serilog)

- `OverdueCheckerHostedService` (BackgroundService) runs `CheckAndPublishOverdueAsync()` every 24h (configurable in appsettings).
- Publishes to queue **`overdue-alerts`**, payload: `borrowRecordId, bookId, bookTitle, memberEmail, memberName, dueDate, daysOverdue`.
- `OverdueAlertConsumerService` (BackgroundService) subscribes at startup; on each message logs at **Warning**:
  `Overdue alert — "{BookTitle}" borrowed by {MemberName} ({MemberEmail}) was due on {DueDate:yyyy-MM-dd} and is {DaysOverdue} day(s) overdue.`
- Undeserialisable message → log **Error** with raw body + **dead-letter** it.

## Database (EF Core code-first; config via `IEntityTypeConfiguration<T>` only)

**Tables:** `Members` (Id, FullName, Email, PasswordHash, Role, IsActive, CreatedAt) · `RefreshTokens` (Id, MemberId FK, Token, ExpiresAt, RevokedAt?) · `Authors` (Id, Name, Biography, Nationality, IsActive, CreatedAt) · `Books` (Id, Title, Description, ISBN, PublicationYear, TotalCopies, AvailableCopies, AuthorId FK, CoverImageUrl?, IsActive, CreatedAt, UpdatedAt) · `BorrowRecords` (Id, MemberId FK, BookId FK, BorrowedAt, DueDate, ReturnedAt?).

**Config requirements:**
- `HasQueryFilter(x => x.IsActive)` on Members, Authors, Books (no manual IsActive checks).
- `HasMaxLength` on all string columns.
- Unique index on `Members.Email` and `Books.ISBN`.
- Index on `BorrowRecords.MemberId`, `.BookId`, `.DueDate`.
- `DeleteBehavior.Restrict` on Book→Author and on BorrowRecord→Book / BorrowRecord→Member.

**Seed data (`DataSeeder`, runs on startup):** 1 Librarian, 2 Members, 3 authors, 8 books (≥2 fully borrowed), 4 borrow records (≥1 overdue).

## Structured logging (Serilog)

Two sinks: Console (coloured in Development) + rolling daily File `logs/lms-.log`. Log: every HTTP request (`HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms`); auth events at Information (memberId; **email only** on login failure); Service Bus publish/consume (borrowRecordId); cache hit/miss at Debug; borrow/return at Information; all exceptions at Error. **Never log passwords, tokens, or raw personal data beyond email and name.**

## Angular frontend (17+, standalone components)

- `AuthService` wraps register/login/logout + silent refresh; JWT in `localStorage`; check expiry before API calls.
- HTTP **Interceptor** attaches Bearer token; on 401 attempt one silent `/auth/refresh`, else clear state → `/login`.
- **Route guards:** `canActivate` on all routes except `/login`, `/register`; separate `LibrarianGuard`.
- **Reactive Forms** via `FormBuilder` + `Validators`, inline per-field errors, submit disabled while invalid/pending.
- Typed services (`BookService`, `AuthorService`, `BorrowService`, `MemberService`) wrap all HTTP — components never inject `HttpClient` directly.
- One UI kit (Angular Material **or** PrimeNG), used consistently. API base URL in `environment.ts` / `environment.prod.ts`.

## Local infrastructure (docker-compose)

`docker-compose.yml` starts: **SQL Server 2022** (`mcr.microsoft.com/mssql/server:2022-latest`), **Redis 7** (`redis:7-alpine`), **Azurite** (`mcr.microsoft.com/azure-storage/azurite`), **Service Bus emulator** (`mcr.microsoft.com/azure-messaging/servicebus-emulator`). All connection strings in `appsettings.Development.json` point at these containers. **Switching to real Azure must require only environment-variable changes — no code changes.** Commit `.env.example` with placeholders; **no real secrets**.

## Common commands (once scaffolded)

```bash
# Infra
docker-compose up -d

# Backend (run from solution root)
dotnet build
dotnet ef migrations add <Name> --project LMS.Infrastructure --startup-project LMS.API
dotnet ef database update --project LMS.Infrastructure --startup-project LMS.API
dotnet run --project LMS.API          # Swagger at /swagger

# Frontend
npm install
ng serve
```

> Note: this shell is **PowerShell** (use `;` not `&&` to chain). The Bash tool is also available for POSIX scripts.

## Deliverables & ground rules

- Individual GitHub repo (no shared repos; similarity detection is run). Backend 4 projects + Angular app + `docker-compose.yml` + `.env.example`.
- Swagger UI at `/swagger` with JWT scheme + DTO docs; committed **Postman collection** covering all endpoints.
- EF migrations + `DataSeeder`. **Conventional commits** (`feat:`, `fix:`, `refactor:`, `chore:`), ≥1 commit/day — history is reviewed.
- Finish all **mandatory** requirements before any bonus. Be ready to explain any line during the walkthrough.

## Scaffolding status & decisions (as of 2026-06-18)

The full **structure is scaffolded and both projects build clean**. Implementation is being
filled in feature by feature — handlers, repositories, EF configurations, and infrastructure
services are stubs (`throw new NotImplementedException()` / `// TODO`); thin controllers,
`Program.cs` wiring, the exception middleware, auth interceptor, guards, and login/register
forms are already functional.

Concrete decisions made during scaffolding (keep consistent):
- **Targets:** .NET 10 (`net10.0`), Angular 21 + Angular Material (azure-blue theme).
- **Key packages:** MediatR 14, AutoMapper 16, FluentValidation 12; **Swashbuckle pinned to 6.6.2**
  (Swashbuckle 10 / Microsoft.OpenApi 2.0 moved the `OpenApiInfo`/security-scheme API — the
  `Microsoft.AspNetCore.OpenApi` template package was removed to avoid the transitive conflict).
- **Entity keys are `Guid`** (`BaseEntity` holds `Id` + `CreatedAt`).
- **CQRS file layout:** one folder per use-case under `Features/<Area>/{Commands,Queries}/<UseCase>/`
  with separate `*Command.cs` / `*Handler.cs` / `*Validator.cs` files. DTOs in `Features/<Area>/DTOs`.
- **Repos return entities;** AutoMapper-to-DTO happens in handlers. `IUnitOfWork.SaveChangesAsync`
  wraps EF; repository `Update`/`MarkReturned` are synchronous (change-tracking only).
- **Extra supporting interfaces** beyond the spec list: `ICurrentUserService` (implemented in API
  over `IHttpContextAccessor`) and `IRefreshTokenRepository`.
- **DI:** interface→implementation bindings live in `LMS.API/Program.cs` (per spec). `AddApplication()`
  only wires MediatR/AutoMapper/validators/pipeline. There is intentionally **no** `AddInfrastructure`
  extension — bindings are explicit in `Program.cs`.
- Background-service stubs are set to `BackgroundServiceExceptionBehavior.Ignore` and seeding is
  wrapped in try/catch so the host boots while internals are unimplemented. **Remove both guards
  once those pieces are implemented.**
- Frontend: standalone components, Angular 21 naming (no `.component` suffix), lazy `loadComponent`
  routes, signal-based `AuthService`, functional `authInterceptor` + `authGuard`/`librarianGuard`.

EF migrations have **not** been generated yet (entity configurations are still stubs). Generate the
initial migration once `IEntityTypeConfiguration` classes are implemented.

## Working conventions for the assistant

- **Never violate the dependency rule** — before adding a package or `using`, confirm it belongs in that layer.
- Validators in Application, EF config in Infrastructure (`IEntityTypeConfiguration`), DI only in `Program.cs`.
- Map with AutoMapper; validate with FluentValidation; route everything through MediatR handlers.
- Use exact key formats, queue names, paths, TTLs, and log message templates specified above — the review checks them literally.
