namespace LMS.Application.Common.Models;

/// <summary>A signed JWT access token plus the moment it expires.</summary>
public record AccessTokenResult(string Token, DateTime ExpiresAt);

/// <summary>An opaque refresh token plus the moment it expires.</summary>
public record RefreshTokenResult(string Token, DateTime ExpiresAt);
