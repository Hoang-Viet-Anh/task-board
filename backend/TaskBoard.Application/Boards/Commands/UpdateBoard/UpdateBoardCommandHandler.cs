using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.Boards.Commands.UpdateBoard;

public record UpdateBoardCommand(Guid UserId, BoardDto BoardDto) : IRequest<Result<Unit>>;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;

    public UpdateBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.BoardDto.Title)) return Result<Unit>.Failure(new BadRequestException("Title is required to update board."));

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());

        var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == request.BoardDto.Id, cancellationToken: cancellationToken);

        if (board == null) return Result<Unit>.Failure(new NotFoundException("Board not found."));
        if (board.OwnerId != user.Id) return Result<Unit>.Failure(new ForbiddenException());

        board.Title = request.BoardDto.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}