using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    // TODO: load (NotFound if missing); ISBN uniqueness excluding self; update; set UpdatedAt; invalidate "books:" cache.
    public Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
