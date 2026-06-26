using MediatR;

namespace LMS.Application.Features.Books.Commands.UploadBookCover;

public class UploadBookCoverCommandHandler : IRequestHandler<UploadBookCoverCommand, string>
{
    // TODO: delete old cover if present; upload via ICoverImageService to covers/{bookId}/{guid}_{filename};
    //       store returned URL; invalidate "books:" cache.
    public Task<string> Handle(UploadBookCoverCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
