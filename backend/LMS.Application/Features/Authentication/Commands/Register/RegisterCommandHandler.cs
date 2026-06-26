using LMS.Application.Features.Authentication.DTOs;
using MediatR;

namespace LMS.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResultDto>
{
    // TODO: hash password (BCrypt via Member entity), persist Member, issue tokens, log "member registered".
    public Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
