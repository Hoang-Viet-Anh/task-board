using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.Columns.Commands.UpdateColumn;

public record UpdateColumnCommand(Guid UserId, ColumnDto ColumnDto) : IRequest<Result<Unit>>;

public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;

    public UpdateColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ColumnDto.Title?.Trim()) && string.IsNullOrEmpty(request.ColumnDto.Order?.Trim())) return Result<Unit>.Failure(new BadRequestException("Title or Order is required"));

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());
        if (request.ColumnDto.Id == null) return Result<Unit>.Failure(new BadRequestException());

        var column = await _context.Columns
        .Include(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .FirstOrDefaultAsync(c => c.Id == request.ColumnDto.Id, cancellationToken: cancellationToken);

        if (column == null) return Result<Unit>.Failure(new NotFoundException("Column not found"));

        if (!column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<Unit>.Failure(new ForbiddenException());

        column.Title = request.ColumnDto.Title ?? column.Title;
        column.Order = request.ColumnDto.Order ?? column.Order;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}