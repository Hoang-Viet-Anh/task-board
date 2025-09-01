using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Columns.Commands.DeleteColumn;

public record DeleteColumnCommand(Guid UserId, Guid ColumnId) : IRequest<Unit>;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public DeleteColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var column = await _context.Columns
            .Include(c => c.Board)
            .ThenInclude(b => b.UserBoards)
            .FirstOrDefaultAsync(c => c.Id == request.ColumnId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Column not found");

        if (!column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        _context.Columns.Remove(column);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}