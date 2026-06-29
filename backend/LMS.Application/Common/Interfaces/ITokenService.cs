using LMS.Application.Common.Models;
using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface ITokenService
{
    /// <summary>Issues a signed JWT access token (with its expiry) for the given member.</summary>
    AccessTokenResult GenerateAccessToken(Member member);

    /// <summary>Issues a new opaque refresh token (with its expiry).</summary>
    RefreshTokenResult GenerateRefreshToken();

    /// <summary>True if the refresh token is present in the Redis revocation set (revoked:{token}).</summary>
    Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken = default);
}
