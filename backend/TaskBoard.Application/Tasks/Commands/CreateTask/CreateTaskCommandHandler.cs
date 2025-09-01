using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Enums;
using TaskEntity = TaskBoard.Domain.Entities.Task;

namespace TaskBoard.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(Guid UserId, TaskDto TaskDto) : IRequest<Unit>;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public CreateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        if (request.TaskDto.ColumnId == null) throw new BadRequestException();
        var column = await _context.Columns
            .Include(c => c.Board)
            .ThenInclude(b => b.UserBoards)
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.TaskDto.ColumnId), cancellationToken: cancellationToken) ?? throw new NotFoundException("Column not found");

        if (!column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        var task = new TaskEntity
        {
            Title = request.TaskDto.Title!,
            Description = request.TaskDto.Description!,
            DueDate = request.TaskDto.DueDate ?? DateTime.UtcNow,
            Priority = Enum.Parse<TaskPriority>(request.TaskDto.Priority!.ToLower()),
            Column = column,
            ColumnId = column.Id
        };
        await _context.Tasks.AddAsync(task, cancellationToken: cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}