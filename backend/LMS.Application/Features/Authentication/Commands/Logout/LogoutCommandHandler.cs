using LMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ICacheService _cache;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ICacheService cache,
        IUnitOfWork unitOfWork,
        ILogger<LogoutCommandHandler> logger)
    {
        _refreshTokens = refreshTokens;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var stored = await _refreshTokens.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (stored is null)
        {
            return; // nothing to revoke — treat logout as idempotent
        }

        // Mark revoked in the database...
        stored.RevokedAt = DateTime.UtcNow;
        _refreshTokens.Update(stored);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ...and add it to the Redis revocation set for the token's remaining lifetime.
        var remaining = stored.ExpiresAt - DateTime.UtcNow;
        if (remaining > TimeSpan.Zero)
        {
            await _cache.SetAsync($"revoked:{request.RefreshToken}", "revoked", remaining, cancellationToken);
        }

        _logger.LogInformation("Logout: {MemberId}", stored.MemberId);
    }
}
