using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authentication.DTOs;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResultDto>
{
    private readonly IMemberRepository _members;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IMemberRepository members,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        ILogger<RegisterCommandHandler> logger)
    {
        _members = members;
        _refreshTokens = refreshTokens;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var existing = await _members.GetByEmailAsync(email, cancellationToken);
        if (existing is not null)
        {
            throw new BusinessRuleException("A member with this email already exists.");
        }

        var member = new Member
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = UserRole.Member,         // public registration is always a Member
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _members.CreateAsync(member, cancellationToken);

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

        _logger.LogInformation("Member registered: {MemberId}", member.Id);

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
