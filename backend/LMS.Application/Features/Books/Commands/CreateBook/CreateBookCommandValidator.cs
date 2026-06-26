using FluentValidation;

namespace LMS.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        // TODO: Title required max 250. Description max 2000. ISBN required, exactly 13 digits.
        // TODO: PublicationYear sane range. TotalCopies min 1. AuthorId required.
    }
}
