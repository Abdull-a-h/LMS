using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<PagedResult<Member>> GetAllAsync(string? keyword, int page, int pageSize, CancellationToken cancellationToken = default);
    Task CreateAsync(Member member, CancellationToken cancellationToken = default);
    void Update(Member member);
    Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
}
