using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Boards.Commands.UpdateBoard;

public record UpdateBoardCommand(Guid UserId, BoardDto BoardDto) : IRequest<Unit>;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public UpdateBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == Guid.Parse(request.BoardDto.Id), cancellationToken: cancellationToken) ?? throw new NotFoundException("Board not found.");

        if (board.OwnerId != user.Id) throw new ForbiddenException();

        if (string.IsNullOrWhiteSpace(request.BoardDto.Title)) return Unit.Value;

        board.Title = request.BoardDto.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}