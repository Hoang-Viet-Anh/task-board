using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Columns.Commands.CreateColumn;

public record CreateColumnCommand(Guid UserId, ColumnDto ColumnDto) : IRequest<Result<Unit>>;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;

    public CreateColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ColumnDto.Title?.Trim())) return Result<Unit>.Failure(new BadRequestException("Column title is required."));

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());
        if (request.ColumnDto.BoardId == null) return Result<Unit>.Failure(new BadRequestException("Board Id is required."));

        var board = await _context.Boards
        .Include(b => b.UserBoards)
        .FirstOrDefaultAsync(b => b.Id == request.ColumnDto.BoardId, cancellationToken: cancellationToken);

        if (board == null) return Result<Unit>.Failure(new NotFoundException("Board is not found"));
        if (!board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<Unit>.Failure(new ForbiddenException());

        if (request.ColumnDto.Title != null)
        {
            var column = new Column
            {
                Title = request.ColumnDto.Title,
                BoardId = board.Id,
                Board = board,
                Order = request.ColumnDto.Order!
            };
            await _context.Columns.AddAsync(column, cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}