using FluentValidation;
using TaskBoard.Application.Boards.Dtos;

namespace TaskBoard.Application.Boards.Validators;

public class UpdateBoardRequestValidator : AbstractValidator<UpdateBoardRequest>
{
    public UpdateBoardRequestValidator()
    {
        RuleFor(x => x.BoardId)
            .NotEmpty().WithMessage("Board id is required.");
    }
}