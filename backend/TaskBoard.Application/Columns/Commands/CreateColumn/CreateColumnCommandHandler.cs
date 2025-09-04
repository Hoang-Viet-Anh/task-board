using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Columns.Commands.CreateColumn;

public record CreateColumnCommand(Guid UserId, ColumnDto ColumnDto) : IRequest<Unit>;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public CreateColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        if (request.ColumnDto.BoardId == null) throw new BadRequestException();
        var board = await _context.Boards
        .Include(b => b.UserBoards)
        .Include(b => b.Columns)
        .FirstOrDefaultAsync(b => b.Id == Guid.Parse(request.ColumnDto.BoardId), cancellationToken: cancellationToken) ?? throw new NotFoundException("Board is not found");

        if (!board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        var lastColumn = board.Columns.OrderBy(c => c.Order).LastOrDefault();
        var order = lastColumn != null ? lastColumn.Order + 1000 : 1000;

        if (request.ColumnDto.Title != null)
        {
            var column = new Column
            {
                Title = request.ColumnDto.Title,
                BoardId = board.Id,
                Board = board,
                Order = order
            };
            await _context.Columns.AddAsync(column, cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}