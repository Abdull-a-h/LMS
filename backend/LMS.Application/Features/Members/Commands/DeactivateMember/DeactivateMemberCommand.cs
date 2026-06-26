using MediatR;

namespace LMS.Application.Features.Members.Commands.DeactivateMember;

public record DeactivateMemberCommand(Guid Id) : IRequest;
