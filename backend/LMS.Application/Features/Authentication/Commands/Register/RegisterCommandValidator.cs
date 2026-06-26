using FluentValidation;

namespace LMS.Application.Features.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        // TODO: FullName required, max 120.
        // TODO: Email required + valid format.
        // TODO: Password min 8 chars, >= 1 uppercase, >= 1 digit.
    }
}
