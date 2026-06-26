using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Persistence.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly LmsDbContext _db;

    public BorrowRepository(LmsDbContext db) => _db = db;

    public Task CreateAsync(BorrowRecord record, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<IReadOnlyList<BorrowRecord>> GetActiveByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<int> CountActiveByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<BorrowRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<PagedResult<BorrowRecord>> GetByMemberPagedAsync(Guid memberId, int page, int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<PagedResult<BorrowRecord>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public void MarkReturned(BorrowRecord record)
        => throw new NotImplementedException();

    public Task<IReadOnlyList<BorrowRecord>> GetOverdueAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
