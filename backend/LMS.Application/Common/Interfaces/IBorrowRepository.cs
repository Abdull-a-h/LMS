using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface IBorrowRepository
{
    Task CreateAsync(BorrowRecord record, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BorrowRecord>> GetActiveByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<int> CountActiveByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<BorrowRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<BorrowRecord>> GetByMemberPagedAsync(Guid memberId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<BorrowRecord>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    void MarkReturned(BorrowRecord record);
    Task<IReadOnlyList<BorrowRecord>> GetOverdueAsync(CancellationToken cancellationToken = default);
}
