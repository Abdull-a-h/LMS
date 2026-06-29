using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Entities;
using LMS.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infrastructure.Identity;

/// <summary>
/// Issues JWT access tokens (claims: userId, email, role) and opaque refresh tokens.
/// Refresh-token revocation is checked against Redis (key: revoked:{token}).
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly ICacheService _cache;

    public TokenService(IOptions<JwtOptions> options, ICacheService cache)
    {
        _options = options.Value;
        _cache = cache;
    }

    public AccessTokenResult GenerateAccessToken(Member member)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
            new Claim(ClaimTypes.Email, member.Email),
            new Claim(ClaimTypes.Role, member.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var encoded = new JwtSecurityTokenHandler().WriteToken(token);
        return new AccessTokenResult(encoded, expiresAt);
    }

    public RefreshTokenResult GenerateRefreshToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenDays);
        return new RefreshTokenResult(token, expiresAt);
    }

    public async Task<bool> IsRefreshTokenRevokedAsync(string token, CancellationToken cancellationToken = default)
    {
        var marker = await _cache.GetAsync<string>($"revoked:{token}", cancellationToken);
        return marker is not null;
    }
}
