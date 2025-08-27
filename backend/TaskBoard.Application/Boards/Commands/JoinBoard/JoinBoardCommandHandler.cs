using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Boards.Commands.JoinBoard;

public record JoinBoardCommand(Guid UserId, string InviteCode) : IRequest<Unit>;

public class JoinBoardCommandHandler : IRequestHandler<JoinBoardCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public JoinBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(JoinBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var board = await _context.Boards.FirstOrDefaultAsync(b => b.InviteCode == request.InviteCode, cancellationToken: cancellationToken) ?? throw new NotFoundException("Board not found.");

        var isAlreadyJoined = await _context.UserBoards.AnyAsync(ub => ub.BoardId == board.Id && ub.UserId == user.Id, cancellationToken: cancellationToken);

        if (isAlreadyJoined) return Unit.Value;

        var userBoard = new UserBoard
        {
            User = user,
            UserId = user.Id,
            Board = board,
            BoardId = board.Id
        };

        await _context.UserBoards.AddAsync(userBoard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}