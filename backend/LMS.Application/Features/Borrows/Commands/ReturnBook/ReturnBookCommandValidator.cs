using FluentValidation;

namespace LMS.Application.Features.Borrows.Commands.ReturnBook;

public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        // TODO: BorrowRecordId required.
    }
}
