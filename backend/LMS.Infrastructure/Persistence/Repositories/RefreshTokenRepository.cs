using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly LmsDbContext _db;

    public RefreshTokenRepository(LmsDbContext db) => _db = db;

    public Task CreateAsync(RefreshToken token, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public void Update(RefreshToken token)
        => throw new NotImplementedException();
}
