using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskBoard.Application.ActivityLogs.Commands.CreateLog;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid UserId, Guid TaskId) : IRequest<Result<Unit>>;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMediator _mediator;
    public readonly IConfiguration _configuration;

    public DeleteTaskCommandHandler(IApplicationDbContext context, IMediator mediator, IConfiguration configuration)
    {
        _context = context;
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<Result<Unit>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<Unit>.Failure(new UnauthorizedAccessException());

        var task = await _context.Tasks
                .Include(t => t.Column)
                .ThenInclude(c => c.Board)
                .ThenInclude(b => b.UserBoards)
                .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken: cancellationToken);

        if (task == null) return Result<Unit>.Success(Unit.Value);
        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<Unit>.Failure(new ForbiddenException());

        _context.Tasks.Remove(task);

        var frontendUrl = _configuration["Frontend:Url"] ?? "";
        var taskUrl = $"{frontendUrl}/board/{task.Column.BoardId}/task/{task.Id}";

        var log = new TaskActivityLog
        {
            Board = task.Column.Board,
            BoardId = task.Column.BoardId,
            Task = task,
            TaskId = task.Id,
            User = user,
            UserId = user.Id,
            Log = $"*{user.Username}* removed _<{taskUrl}|{task.Title}>_"
        };
        var command = new CreateLogCommand(log);
        var result = await _mediator.Send(command, cancellationToken);

        return result;
    }
}