using FluentValidation;

namespace LMS.Application.Features.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        // TODO: Id required. Name required, max 150. Biography max 1000.
    }
}
