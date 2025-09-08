using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.Tasks.Queries.GetTaskById;

public record GetTaskByIdQuery(Guid UserId, Guid TaskId) : IRequest<Result<TaskDto>>;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<TaskDto>.Failure(new UnauthorizedAccessException());

        var task = await _context.Tasks
        .Include(t => t.UserTasks)
        .ThenInclude(ut => ut.User)
        .Include(t => t.Column)
        .ThenInclude(c => c.Board)
        .ThenInclude(b => b.UserBoards)
        .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken: cancellationToken);

        if (task == null) return Result<TaskDto>.Failure(new NotFoundException("Task not found"));
        if (!task.Column.Board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<TaskDto>.Failure(new ForbiddenException());

        var taskDto = _mapper.Map<TaskDto>(task);

        return Result<TaskDto>.Success(taskDto);
    }
}