using MediatR;

namespace LMS.Application.Features.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
{
    // TODO: soft-delete; reject (BusinessRuleException) if the author has active books.
    public Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
