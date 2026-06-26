using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LmsDbContext _db;

    public MemberRepository(LmsDbContext db) => _db = db;

    public Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<PagedResult<Member>> GetAllAsync(string? keyword, int page, int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task CreateAsync(Member member, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public void Update(Member member)
        => throw new NotImplementedException();

    public Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
