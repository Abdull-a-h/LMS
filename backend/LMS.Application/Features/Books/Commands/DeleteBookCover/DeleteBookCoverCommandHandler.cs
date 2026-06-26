using MediatR;

namespace LMS.Application.Features.Books.Commands.DeleteBookCover;

public class DeleteBookCoverCommandHandler : IRequestHandler<DeleteBookCoverCommand>
{
    // TODO: delete blob via ICoverImageService; clear CoverImageUrl; invalidate "books:" cache.
    public Task Handle(DeleteBookCoverCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
