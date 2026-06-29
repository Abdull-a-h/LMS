using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly LmsDbContext _db;

    public RefreshTokenRepository(LmsDbContext db) => _db = db;

    public async Task CreateAsync(RefreshToken token, CancellationToken cancellationToken = default)
        => await _db.RefreshTokens.AddAsync(token, cancellationToken);

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        => _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

    public void Update(RefreshToken token)
        => _db.RefreshTokens.Update(token);
}
