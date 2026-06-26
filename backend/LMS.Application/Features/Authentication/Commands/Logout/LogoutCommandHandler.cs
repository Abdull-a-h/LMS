using MediatR;

namespace LMS.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    // TODO: write refresh token to Redis (revoked:{token}, TTL = remaining lifetime); log "logout".
    public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
