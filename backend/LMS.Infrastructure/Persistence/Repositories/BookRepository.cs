using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LmsDbContext _db;

    public BookRepository(LmsDbContext db) => _db = db;

    // Tracked load including the author (needed for AuthorName / nested AuthorDto and
    // for mutations in update/delete/cover handlers). IsActive query filter excludes
    // soft-deleted books automatically.
    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<PagedResult<Book>> GetPagedAsync(
        Guid? authorId, string? keyword, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        // Defensive clamping so a bad query string can't request page 0 or a huge page.
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var query = _db.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .AsQueryable();

        if (authorId is not null)
        {
            query = query.Where(b => b.AuthorId == authorId);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var term = keyword.Trim();
            // SQL Server's default collation is case-insensitive, so Contains matches both cases.
            query = query.Where(b => b.Title.Contains(term) || b.ISBN.Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Book>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    // IgnoreQueryFilters so a soft-deleted book's ISBN still counts as taken — the DB
    // unique index spans all rows, so this prevents a confusing DB-level violation.
    public Task<bool> IsbnExistsAsync(string isbn, Guid? excludeBookId = null, CancellationToken cancellationToken = default)
        => _db.Books
            .IgnoreQueryFilters()
            .AnyAsync(b => b.ISBN == isbn && (excludeBookId == null || b.Id != excludeBookId), cancellationToken);

    public async Task CreateAsync(Book book, CancellationToken cancellationToken = default)
        => await _db.Books.AddAsync(book, cancellationToken);

    public void Update(Book book)
        => _db.Books.Update(book);

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (book is not null)
        {
            book.IsActive = false;
        }
    }
}
