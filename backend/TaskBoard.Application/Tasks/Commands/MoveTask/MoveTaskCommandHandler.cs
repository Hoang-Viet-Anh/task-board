using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Tasks.Commands.MoveTask;

public record MoveTaskCommand(Guid UserId, TaskDto TaskDto) : IRequest<Unit>;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, Unit>
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

    public async Task<Unit> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
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
        await _mediator.Send(command, cancellationToken);

        return Unit.Value;
    }
}