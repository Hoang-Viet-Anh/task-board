using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Tasks.Commands.MoveTask;

public record MoveTaskCommand(Guid UserId, TaskDto TaskDto) : IRequest<Result<Unit>>;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMediator _mediator;
    public readonly IConfiguration _configuration;

    public MoveTaskCommandHandler(IApplicationDbContext context, IMediator mediator, IConfiguration configuration)
    {
        _context = context;
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<Result<Unit>> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());
        if (request.TaskDto.ColumnId == null) return Result<Unit>.Failure(new BadRequestException());

        var column = await _context.Columns.FirstOrDefaultAsync(c => c.Id == request.TaskDto.ColumnId, cancellationToken: cancellationToken);

        if (column == null) return Result<Unit>.Failure(new NotFoundException("Column not found"));

        var task = await _context.Tasks
        .Include(t => t.Column)
        .ThenInclude(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .FirstOrDefaultAsync(t => t.Id == request.TaskDto.Id, cancellationToken: cancellationToken);

        if (task == null) return Result<Unit>.Failure(new NotFoundException("Task not found"));
        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<Unit>.Failure(new ForbiddenException());

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
            Log = $"*{user.Username}* moved _<{taskUrl}|{task.Title}>_ from *{task.Column.Title}* to *{column.Title}*"
        };

        task.ColumnId = request.TaskDto.ColumnId ?? task.ColumnId;
        task.Column = column;

        var command = new CreateLogCommand(log);
        var result = await _mediator.Send(command, cancellationToken);

        return result;
    }
}