using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Columns.Commands.UpdateColumn;

public record UpdateColumnCommand(Guid UserId, ColumnDto ColumnDto) : IRequest<Unit>;

public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public UpdateColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        if (request.ColumnDto.Id == null) throw new BadRequestException();
        var column = await _context.Columns
        .Include(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.ColumnDto.Id), cancellationToken: cancellationToken) ?? throw new NotFoundException("Column not found");

        if (!column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        column.Title = request.ColumnDto.Title ?? column.Title;
        column.Order = request.ColumnDto.Order ?? column.Order;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}