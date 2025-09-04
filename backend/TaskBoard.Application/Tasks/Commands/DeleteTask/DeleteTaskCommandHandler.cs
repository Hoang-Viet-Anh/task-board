using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskEntity = TaskBoard.Domain.Entities.Task;

namespace TaskBoard.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid UserId, Guid TaskId) : IRequest<Unit>;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
{
    public readonly IApplicationDbContext _context;
    public readonly IMediator _mediator;

    public DeleteTaskCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
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

        var log = new TaskActivityLog
        {
            Board = task.Column.Board,
            BoardId = task.Column.BoardId,
            Task = task,
            TaskId = task.Id,
            User = user,
            UserId = user.Id,
            Log = $"{user.Username} removed \"{task.Title}\""
        };
        var command = new CreateLogCommand(log);
        await _mediator.Send(command, cancellationToken);

        return Unit.Value;
    }
}