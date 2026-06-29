using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authentication.DTOs;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly IMemberRepository _members;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IMemberRepository members,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        ILogger<LoginCommandHandler> logger)
    {
        _members = members;
        _refreshTokens = refreshTokens;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        // The IsActive query filter means deactivated members resolve to null here — they cannot log in.
        var member = await _members.GetByEmailAsync(email, cancellationToken);
        if (member is null || !_passwordHasher.Verify(request.Password, member.PasswordHash))
        {
            _logger.LogInformation("Login failed for {Email}", email);
            throw new InvalidCredentialsException("Invalid email or password.");
        }

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

        _logger.LogInformation("Login success: {MemberId}", member.Id);

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
