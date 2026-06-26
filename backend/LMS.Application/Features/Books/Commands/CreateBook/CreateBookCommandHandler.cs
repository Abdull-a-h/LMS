using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    // TODO: check ISBN uniqueness; AvailableCopies = TotalCopies; persist; invalidate "books:" cache.
    public Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
