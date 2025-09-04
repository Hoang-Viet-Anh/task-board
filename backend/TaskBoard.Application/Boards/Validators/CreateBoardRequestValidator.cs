using FluentValidation;
using TaskBoard.Application.Boards.Commands.CreateBoard;

namespace TaskBoard.Application.Boards.Validators;

public class CreateBoardRequestValidator : AbstractValidator<CreateBoardRequest>
{
    public CreateBoardRequestValidator()
    {
        RuleFor(x => x.BoardTitle)
            .NotEmpty().WithMessage("Board title is required.")
            .MinimumLength(3).WithMessage("Board title must at least be 3 character long")
            .MaximumLength(32).WithMessage("Board title can not be bigger than 32 characters");
    }
}