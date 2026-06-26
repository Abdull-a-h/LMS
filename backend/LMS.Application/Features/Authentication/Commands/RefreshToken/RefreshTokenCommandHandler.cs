using LMS.Application.Features.Authentication.DTOs;
using MediatR;

namespace LMS.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    // TODO: reject if token in Redis revocation set (revoked:{token}); else issue new access token + rotate refresh token.
    public Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
