using LMS.Application.Features.Authentication.DTOs;
using MediatR;

namespace LMS.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    // TODO: verify credentials, reject inactive members, issue access + refresh tokens, persist refresh token, log success/failure.
    public Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
