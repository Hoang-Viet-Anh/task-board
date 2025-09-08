using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskEntity = TaskBoard.Domain.Entities.Task;

namespace TaskBoard.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(Guid UserId, TaskDto TaskDto) : IRequest<Result<Unit>>;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMediator _mediator;
    public readonly IConfiguration _configuration;

    public CreateTaskCommandHandler(IApplicationDbContext context, IMediator mediator, IConfiguration configuration)
    {
        _context = context;
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<Result<Unit>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());
        if (request.TaskDto.ColumnId == null) return Result<Unit>.Failure(new BadRequestException());

        var column = await _context.Columns
            .Include(c => c.Board)
            .ThenInclude(b => b.UserBoards)
            .FirstOrDefaultAsync(c => c.Id == request.TaskDto.ColumnId, cancellationToken: cancellationToken);

        if (column == null) return Result<Unit>.Failure(new NotFoundException("Column not found"));
        if (!column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<Unit>.Failure(new ForbiddenException());

        var task = new TaskEntity
        {
            Title = request.TaskDto.Title!,
            Description = request.TaskDto.Description!,
            DueDate = request.TaskDto.DueDate ?? DateTime.UtcNow,
            Priority = Enum.Parse<TaskPriority>(request.TaskDto.Priority!.ToUpper()),
            Column = column,
            ColumnId = column.Id
        };
        await _context.Tasks.AddAsync(task, cancellationToken: cancellationToken);

        var frontendUrl = _configuration["Frontend:Url"] ?? "";
        var taskUrl = $"{frontendUrl}/board/{task.Column.BoardId}/task/{task.Id}";

        var log = new TaskActivityLog
        {
            Board = column.Board,
            BoardId = column.BoardId,
            Task = task,
            TaskId = task.Id,
            User = user,
            UserId = user.Id,
            Log = $"*{user.Username}* created _<{taskUrl}|{task.Title}>_"
        };
        var command = new CreateLogCommand(log);
        var result = await _mediator.Send(command, cancellationToken);

        return result;
    }
}