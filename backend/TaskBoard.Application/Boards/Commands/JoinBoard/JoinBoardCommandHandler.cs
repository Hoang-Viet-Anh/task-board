using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Boards.Commands.JoinBoard;

public record JoinBoardCommand(Guid UserId, string InviteCode) : IRequest<Result<Unit>>;

public class JoinBoardCommandHandler : IRequestHandler<JoinBoardCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;

    public JoinBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(JoinBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());

        var board = await _context.Boards.FirstOrDefaultAsync(b => b.InviteCode == request.InviteCode, cancellationToken: cancellationToken);

        if (board == null) return Result<Unit>.Failure(new NotFoundException("Board not found."));

        var isAlreadyJoined = await _context.UserBoards.AnyAsync(ub => ub.BoardId == board.Id && ub.UserId == user.Id, cancellationToken: cancellationToken);

        if (isAlreadyJoined) return Result<Unit>.Success(Unit.Value);

        var userBoard = new UserBoard
        {
            User = user,
            UserId = user.Id,
            Board = board,
            BoardId = board.Id
        };

        await _context.UserBoards.AddAsync(userBoard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}