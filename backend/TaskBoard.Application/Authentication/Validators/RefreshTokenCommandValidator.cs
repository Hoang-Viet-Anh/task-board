using FluentValidation;
using TaskBoard.Application.Authentication.Commands.RefreshToken;

namespace TaskBoard.Application.Authentication.Validators;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}