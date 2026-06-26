using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface IAuthorRepository
{
    Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(Author author, CancellationToken cancellationToken = default);
    void Update(Author author);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
