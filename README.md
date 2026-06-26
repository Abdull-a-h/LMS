# Library Management System (LMS)

A web application where a **Librarian** manages books, authors, and members, and **Members**
browse, borrow, and return books. Overdue books trigger asynchronous alerts.

- **Backend:** .NET 10 Web API, Clean Architecture (Domain / Application / Infrastructure / API)
- **Frontend:** Angular 21 (standalone components) + Angular Material
- **Data:** SQL Server + Redis
- **Cloud (local emulators):** Azure Blob Storage (Azurite) + Azure Service Bus emulator

> Full requirements: `Assignment.docx`. Architectural guidance for contributors: `CLAUDE.md`.

## Repository layout

```
backend/                 .NET solution (4 projects)
  LMS.Domain/            Entities, enums, custom exceptions (no dependencies)
  LMS.Application/       CQRS (MediatR), DTOs, AutoMapper, FluentValidation, interfaces
  LMS.Infrastructure/    EF Core, Redis, Blob, Service Bus, background services
  LMS.API/               Controllers, middleware, JWT, Swagger, DI (Program.cs)
frontend/                Angular app (Material)
servicebus/Config.json   Service Bus emulator queue definitions
docker-compose.yml       SQL Server + Redis + Azurite + Service Bus emulator
.env.example             Required environment variables (no real secrets)
```

## Getting started

```bash
# 1. Start local infrastructure
cp .env.example .env        # then edit values as needed
docker compose up -d

# 2. Backend
cd backend
dotnet ef database update --project LMS.Infrastructure --startup-project LMS.API
dotnet run --project LMS.API          # Swagger at https://localhost:<port>/swagger

# 3. Frontend
cd ../frontend
npm install
ng serve                              # http://localhost:4200
```

## Status

Project **structure scaffolded**; feature implementation is in progress. Handlers,
repositories, and infrastructure services are stubbed (`NotImplementedException` / `// TODO`)
and being filled in feature by feature.
