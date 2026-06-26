using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Persistence.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly LmsDbContext _db;

    public AuthorRepository(LmsDbContext db) => _db = db;

    public Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task CreateAsync(Author author, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public void Update(Author author)
        => throw new NotImplementedException();

    public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
