using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.ActivityLogs.Queries.GetLogByBoardId;

public record GetLogByBoardIdQuery(Guid UserId, Guid BoardId, int Page) : IRequest<Result<List<TaskActivityLogDto>>>;

public class GetLogByBoardIdQueryHandler : IRequestHandler<GetLogByBoardIdQuery, Result<List<TaskActivityLogDto>>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;
    public const int pageSize = 20;

    public GetLogByBoardIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<TaskActivityLogDto>>> Handle(GetLogByBoardIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<List<TaskActivityLogDto>>.Failure(new UnauthorizedAccessException());

        var board = await _context.Boards
            .Include(b => b.UserBoards)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken: cancellationToken);

        if (board == null) return Result<List<TaskActivityLogDto>>.Failure(new NotFoundException("Board not found"));

        if (!board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<List<TaskActivityLogDto>>.Failure(new ForbiddenException());

        var activityLogs = await _context.TaskActivityLogs
            .Where(l => l.BoardId == request.BoardId)
            .OrderByDescending(l => l.CreatedAt)
            .Skip(request.Page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        var activityLogsDto = _mapper.Map<List<TaskActivityLogDto>>(activityLogs);

        return Result<List<TaskActivityLogDto>>.Success(activityLogsDto);
    }
}