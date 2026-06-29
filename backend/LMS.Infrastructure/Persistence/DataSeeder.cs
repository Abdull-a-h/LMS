using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence;

/// <summary>
/// Seeds baseline data on startup. For now it seeds the single Librarian account
/// (librarians are seeded, not self-registered). Step 8 expands this to the full
/// demo data set (members, authors, books, borrow records).
/// </summary>
public class DataSeeder
{
    public const string LibrarianEmail = "librarian@lms.local";
    public const string LibrarianPassword = "Librarian123";

    private readonly LmsDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public DataSeeder(LmsDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Members.AnyAsync(m => m.Email == LibrarianEmail, cancellationToken))
        {
            return; // already seeded — keep startup idempotent
        }

        _db.Members.Add(new Member
        {
            Id = Guid.NewGuid(),
            FullName = "Head Librarian",
            Email = LibrarianEmail,
            PasswordHash = _passwordHasher.Hash(LibrarianPassword),
            Role = UserRole.Librarian,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
    }
}
