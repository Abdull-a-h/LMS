using FluentValidation;

namespace LMS.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Book id is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Matches(@"^\d{13}$").WithMessage("ISBN must be exactly 13 digits.");

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1450, DateTime.UtcNow.Year + 1)
            .WithMessage($"Publication year must be between 1450 and {DateTime.UtcNow.Year + 1}.");

        RuleFor(x => x.TotalCopies)
            .GreaterThanOrEqualTo(1).WithMessage("Total copies must be at least 1.");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author is required.");
    }
}
