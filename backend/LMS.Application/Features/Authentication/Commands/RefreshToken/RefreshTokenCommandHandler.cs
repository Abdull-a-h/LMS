using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authentication.DTOs;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IMemberRepository _members;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IMemberRepository members,
        IRefreshTokenRepository refreshTokens,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _members = members;
        _refreshTokens = refreshTokens;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Rejected immediately if the token was logged out (present in the Redis revocation set).
        if (await _tokenService.IsRefreshTokenRevokedAsync(request.RefreshToken, cancellationToken))
        {
            throw new InvalidCredentialsException("Refresh token has been revoked.");
        }

        // 2. Must exist, be unrevoked, and unexpired in the database.
        var stored = await _refreshTokens.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (stored is null || stored.RevokedAt is not null || stored.ExpiresAt <= DateTime.UtcNow)
        {
            throw new InvalidCredentialsException("Refresh token is invalid or expired.");
        }

        // 3. The owning member must still be active.
        var member = await _members.GetByIdAsync(stored.MemberId, cancellationToken);
        if (member is null)
        {
            throw new InvalidCredentialsException("Refresh token is invalid or expired.");
        }

        // 4. Rotate: revoke the old token and issue a fresh access + refresh pair.
        stored.RevokedAt = DateTime.UtcNow;
        _refreshTokens.Update(stored);

        var access = _tokenService.GenerateAccessToken(member);
        var refresh = _tokenService.GenerateRefreshToken();

        await _refreshTokens.CreateAsync(new LMS.Domain.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refresh.Token,
            ExpiresAt = refresh.ExpiresAt,
            MemberId = member.Id,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Token refreshed: {MemberId}", member.Id);

        return new AuthResultDto
        {
            AccessToken = access.Token,
            RefreshToken = refresh.Token,
            AccessTokenExpiresAt = access.ExpiresAt,
            MemberId = member.Id,
            FullName = member.FullName,
            Role = member.Role.ToString()
        };
    }
}
