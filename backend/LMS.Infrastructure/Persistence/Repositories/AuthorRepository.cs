using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly LmsDbContext _db;

    public AuthorRepository(LmsDbContext db) => _db = db;

    // Read-only list; AsNoTracking avoids change-tracker overhead. The IsActive
    // query filter (configured on Author) excludes soft-deleted authors automatically.
    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Authors
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

    // Tracked load including the author's books. Books carry their own IsActive
    // query filter, so only active books are materialised — exactly what both the
    // detail view and the delete guard need. Tracked so Update/SoftDelete can mutate it.
    public Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task CreateAsync(Author author, CancellationToken cancellationToken = default)
        => await _db.Authors.AddAsync(author, cancellationToken);

    // Change-tracking only; the unit of work commits.
    public void Update(Author author)
        => _db.Authors.Update(author);

    // Soft delete: flip IsActive. The query filter then hides the row from all
    // subsequent reads without physically removing it (preserves referential history).
    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (author is not null)
        {
            author.IsActive = false;
        }
    }
}
