using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Infrastructure.Identity;

/// <summary>
/// Issues JWT access tokens (30 min, userId/email/role claims) and opaque refresh tokens (7 days).
/// Refresh-token revocation is checked against Redis (revoked:{token}).
/// </summary>
public class TokenService : ITokenService
{
    // TODO: inject IOptions<JwtOptions> + ICacheService.

    public string GenerateAccessToken(Member member)
        => throw new NotImplementedException();

    public string GenerateRefreshToken()
        => throw new NotImplementedException();

    public Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
