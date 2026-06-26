using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    void Update(RefreshToken token);
}
