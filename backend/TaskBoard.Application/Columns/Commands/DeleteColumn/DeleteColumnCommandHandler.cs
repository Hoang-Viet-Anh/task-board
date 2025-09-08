using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.Columns.Commands.DeleteColumn;

public record DeleteColumnCommand(Guid UserId, Guid ColumnId) : IRequest<Result<Unit>>;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;

    public DeleteColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());

        var column = await _context.Columns
            .Include(c => c.Board)
            .ThenInclude(b => b.UserBoards)
            .FirstOrDefaultAsync(c => c.Id == request.ColumnId, cancellationToken: cancellationToken);

        if (column == null) return Result<Unit>.Failure(new NotFoundException("Column not found"));
        if (!column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<Unit>.Failure(new ForbiddenException());

        _context.Columns.Remove(column);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}