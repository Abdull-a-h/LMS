using LMS.Domain.Entities;

namespace LMS.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Member member);
    string GenerateRefreshToken();
    Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken = default);
}
