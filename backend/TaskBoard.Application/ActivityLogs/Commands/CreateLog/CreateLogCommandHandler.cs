using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskEntity = TaskBoard.Domain.Entities.Task;

namespace TaskBoard.Application.ActivityLogs.Commands.CreateLog;

public record CreateLogCommand(TaskActivityLog Log) : IRequest<Unit>;

public class CreateLogCommandHandler : IRequestHandler<CreateLogCommand, Unit>
{
    public readonly IApplicationDbContext _context;
    public readonly ISlackService _slackService;

    public CreateLogCommandHandler(IApplicationDbContext context, ISlackService slackService)
    {
        _context = context;
        _slackService = slackService;
    }

    public async Task<Unit> Handle(CreateLogCommand request, CancellationToken cancellationToken)
    {
        _context.TaskActivityLogs.Add(request.Log);

        await _context.SaveChangesAsync(cancellationToken);
        await _slackService.SendMessage(request.Log.Log);
        return Unit.Value;
    }
}