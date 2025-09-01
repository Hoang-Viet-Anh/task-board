using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Enums;
using TaskEntity = TaskBoard.Domain.Entities.Task;

namespace TaskBoard.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid UserId, Guid TaskId) : IRequest<Unit>;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var task = await _context.Tasks
            .Include(t => t.Column)
            .ThenInclude(c => c.Board)
            .ThenInclude(b => b.UserBoards)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Task not found");

        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}