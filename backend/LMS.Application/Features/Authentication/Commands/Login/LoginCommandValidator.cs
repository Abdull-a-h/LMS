using FluentValidation;

namespace LMS.Application.Features.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        // TODO: Email required + valid format. Password required.
    }
}
