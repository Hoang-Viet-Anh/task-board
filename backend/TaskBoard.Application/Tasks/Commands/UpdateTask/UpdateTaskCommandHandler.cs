using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(Guid UserId, TaskDto TaskDto) : IRequest<Unit>;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
{
    public readonly IApplicationDbContext _context;
    public readonly IMediator _mediator;
    public readonly IConfiguration _configuration;

    public UpdateTaskCommandHandler(IApplicationDbContext context, IMediator mediator, IConfiguration configuration)
    {
        _context = context;
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        if (request.TaskDto.ColumnId == null) throw new BadRequestException();
        var column = await _context.Columns.FirstOrDefaultAsync(c => c.Id == request.TaskDto.ColumnId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Column not found");

        var task = await _context.Tasks
        .Include(t => t.Column)
        .ThenInclude(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .FirstOrDefaultAsync(t => t.Id == Guid.Parse(request.TaskDto.Id!), cancellationToken: cancellationToken) ?? throw new NotFoundException("Task not found");

        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        var changes = new List<string>();

        if (request.TaskDto.Title != null && request.TaskDto.Title != task.Title)
        {
            changes.Add($"• Title changed from *{task.Title}* to *{request.TaskDto.Title}*");
            task.Title = request.TaskDto.Title;
        }

        if (request.TaskDto.Description != null && request.TaskDto.Description != task.Description)
        {
            changes.Add("• Description updated");
            task.Description = request.TaskDto.Description;
        }

        if (request.TaskDto.DueDate.HasValue && request.TaskDto.DueDate != task.DueDate)
        {
            changes.Add($"• Due date changed from *{task.DueDate:dd.MM.yyyy}* to *{request.TaskDto.DueDate.Value:dd.MM.yyyy}*");
            task.DueDate = (DateTime)request.TaskDto.DueDate;
        }

        if (request.TaskDto.Priority != null)
        {
            var parsedPriority = Enum.Parse<TaskPriority>(request.TaskDto.Priority.ToUpper());
            if (parsedPriority != task.Priority)
            {
                changes.Add($"• Priority changed from *{task.Priority}* to *{parsedPriority}*");
                task.Priority = parsedPriority;
            }
        }

        if (column.Id != task.ColumnId)
        {
            changes.Add($"• Moved from *{task.Column.Title}* to *{column.Title}*");
            task.ColumnId = request.TaskDto.ColumnId ?? task.ColumnId;
            task.Column = column;
        }

        var frontendUrl = _configuration["Frontend:Url"] ?? "";
        var taskUrl = $"{frontendUrl}/board/{task.Column.BoardId}/task/{task.Id}";

        var log = string.Join("\n", changes);
        var activityLog = new TaskActivityLog
        {
            Board = column.Board,
            BoardId = column.BoardId,
            Task = task,
            TaskId = task.Id,
            User = user,
            UserId = user.Id,
            Log = $"*{user.Username}* updated _<{taskUrl}|{task.Title}>_:\n{log}"
        };
        var command = new CreateLogCommand(activityLog);
        await _mediator.Send(command, cancellationToken);


        return Unit.Value;
    }
}