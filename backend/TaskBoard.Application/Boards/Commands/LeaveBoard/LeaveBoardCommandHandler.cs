using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Boards.Commands.LeaveBoard;

public record LeaveBoardCommand(Guid UserId, Guid BoardId) : IRequest<Unit>;

public class LeaveBoardCommandHandler : IRequestHandler<LeaveBoardCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public LeaveBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(LeaveBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken) ?? throw new UnauthorizedAccessException();
        var userBoard = await _context.UserBoards.Include(ub => ub.Board).FirstOrDefaultAsync(ub => ub.UserId == user.Id && ub.BoardId == request.BoardId, cancellationToken);

        if (userBoard == null) return Unit.Value;

        var isOwner = userBoard.Board.OwnerId == user.Id;

        if (isOwner)
        {
            _context.Boards.Remove(userBoard.Board);
        }
        _context.UserBoards.Remove(userBoard);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}