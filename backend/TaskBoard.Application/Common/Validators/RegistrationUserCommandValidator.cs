using FluentValidation;
using TaskBoard.Application.Authentication.Commands;

namespace TaskBoard.Application.Common.Validators;

public class RegistrationUserCommandValidator : AbstractValidator<RegistrationUserCommand>
{
    public RegistrationUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
    }
}