using FluentValidation;

namespace LMS.Application.Features.Borrows.Commands.BorrowBook;

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        // TODO: BookId required.
    }
}
