using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LmsDbContext _db;

    public MemberRepository(LmsDbContext db) => _db = db;

    public Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Members.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _db.Members.FirstOrDefaultAsync(m => m.Email == email, cancellationToken);

    public async Task CreateAsync(Member member, CancellationToken cancellationToken = default)
        => await _db.Members.AddAsync(member, cancellationToken);

    public Task<PagedResult<Member>> GetAllAsync(string? keyword, int page, int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public void Update(Member member)
        => throw new NotImplementedException();

    public Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
