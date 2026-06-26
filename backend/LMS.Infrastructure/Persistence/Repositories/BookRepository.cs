using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LmsDbContext _db;

    public BookRepository(LmsDbContext db) => _db = db;

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<PagedResult<Book>> GetPagedAsync(Guid? authorId, string? keyword, int page, int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<bool> IsbnExistsAsync(string isbn, Guid? excludeBookId = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task CreateAsync(Book book, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public void Update(Book book)
        => throw new NotImplementedException();

    public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
