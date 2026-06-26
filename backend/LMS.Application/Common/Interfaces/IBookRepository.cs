using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Book>> GetPagedAsync(Guid? authorId, string? keyword, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> IsbnExistsAsync(string isbn, Guid? excludeBookId = null, CancellationToken cancellationToken = default);
    Task CreateAsync(Book book, CancellationToken cancellationToken = default);
    void Update(Book book);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
