using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Boards.Commands.CreateBoard;

public record CreateBoardCommand(Guid UserId, string BoardTitle) : IRequest<Result<Unit>>;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;
    public readonly IInviteCodeGenerator _codeGenerator;

    public CreateBoardCommandHandler(IApplicationDbContext context, IInviteCodeGenerator codeGenerator)
    {
        _context = context;
        _codeGenerator = codeGenerator;
    }

    public async Task<Result<Unit>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.BoardTitle.Trim())) return Result<Unit>.Failure(new BadRequestException("Board title is required."));

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());

        string inviteCode = await _codeGenerator.GenerateUniqueInviteCode(cancellationToken);

        var board = new Board
        {
            Title = request.BoardTitle,
            InviteCode = inviteCode,
            Owner = user,
            OwnerId = user.Id
        };

        var userBoard = new UserBoard
        {
            User = user,
            UserId = user.Id,
            Board = board,
            BoardId = board.Id
        };

        await _context.Boards.AddAsync(board, cancellationToken);
        await _context.UserBoards.AddAsync(userBoard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}