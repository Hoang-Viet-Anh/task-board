using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Boards.Commands.CreateBoard;

public record CreateBoardCommand(Guid UserId, string BoardTitle) : IRequest<Unit>;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Unit>
{
    public readonly IApplicationDbContext _context;
    public readonly IInviteCodeGenerator _codeGenerator;

    public CreateBoardCommandHandler(IApplicationDbContext context, IInviteCodeGenerator codeGenerator)
    {
        _context = context;
        _codeGenerator = codeGenerator;
    }

    public async Task<Unit> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
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

        return Unit.Value;
    }
}