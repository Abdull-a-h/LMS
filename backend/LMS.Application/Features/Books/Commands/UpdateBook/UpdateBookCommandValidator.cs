using FluentValidation;

namespace LMS.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        // TODO: same rules as CreateBook + Id required.
    }
}
