using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Tasks.Commands.AssignTask;

public record AssignTaskCommand(Guid UserId, AssignDto AssignDto) : IRequest<Unit>;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, Unit>
{
    public readonly IApplicationDbContext _context;
    public readonly IMediator _mediator;

    public AssignTaskCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();

        var asignee = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.AssignDto.AsigneeId, cancellationToken: cancellationToken) ?? throw new NotFoundException("User not found");

        var task = await _context.Tasks
        .Include(t => t.Column)
        .ThenInclude(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .Include(t => t.UserTasks)
        .FirstOrDefaultAsync(t => t.Id == request.AssignDto.TaskId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Task not found");

        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == asignee.Id)) throw new ForbiddenException();

        string log;

        var assigning = task.UserTasks.Find(ut => ut.UserId == asignee.Id);
        if (assigning != null)
        {
            _context.UserTasks.Remove(assigning);
            log = $"{user.Username} unassigned \"{task.Title}\" from {asignee.Username}";
        }
        else
        {
            var userTask = new UserTask
            {
                Task = task,
                TaskId = task.Id,
                User = asignee,
                UserId = asignee.Id
            };
            _context.UserTasks.Add(userTask);
            log = $"{user.Username} assigned \"{task.Title}\" to {asignee.Username}";
        }


        var activityLog = new TaskActivityLog
        {
            Board = task.Column.Board,
            BoardId = task.Column.BoardId,
            Task = task,
            TaskId = task.Id,
            User = user,
            UserId = user.Id,
            Log = log
        };
        var command = new CreateLogCommand(activityLog);
        await _mediator.Send(command, cancellationToken);


        return Unit.Value;
    }
}