using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(Guid UserId, TaskDto TaskDto) : IRequest<Unit>;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
{
    public readonly IApplicationDbContext _context;

    public UpdateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        if (request.TaskDto.ColumnId == null) throw new BadRequestException();
        var task = await _context.Tasks
        .Include(t => t.Column)
        .ThenInclude(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .FirstOrDefaultAsync(t => t.Id == Guid.Parse(request.TaskDto.Id!), cancellationToken: cancellationToken) ?? throw new NotFoundException("Task not found");

        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        task.Title = request.TaskDto.Title ?? task.Title;
        task.Description = request.TaskDto.Description ?? task.Description;
        task.DueDate = request.TaskDto.DueDate ?? task.DueDate;
        task.Priority = request.TaskDto.Priority != null ? Enum.Parse<TaskPriority>(request.TaskDto.Priority.ToLower()) : task.Priority;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}