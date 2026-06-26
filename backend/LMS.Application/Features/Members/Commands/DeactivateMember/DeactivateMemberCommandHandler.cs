using MediatR;

namespace LMS.Application.Features.Members.Commands.DeactivateMember;

public class DeactivateMemberCommandHandler : IRequestHandler<DeactivateMemberCommand>
{
    // TODO: reject (BusinessRuleException) if member has active borrows; else soft-deactivate.
    public Task Handle(DeactivateMemberCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
