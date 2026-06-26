using FluentValidation;

namespace LMS.Application.Features.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        // TODO: Name required, max 150. Biography max 1000.
    }
}
