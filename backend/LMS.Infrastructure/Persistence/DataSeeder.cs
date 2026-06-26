namespace LMS.Infrastructure.Persistence;

/// <summary>
/// Seeds baseline data on startup: 1 Librarian, 2 Members, 3 authors, 8 books
/// (>= 2 fully borrowed), 4 borrow records (>= 1 overdue to demo the alert flow).
/// </summary>
public class DataSeeder
{
    private readonly LmsDbContext _db;

    public DataSeeder(LmsDbContext db) => _db = db;

    public Task SeedAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
