using MediatR;

namespace LMS.Application.Features.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    // TODO: reject (BusinessRuleException) if active borrows exist; delete cover blob if present; soft-delete; invalidate "books:" cache.
    public Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
