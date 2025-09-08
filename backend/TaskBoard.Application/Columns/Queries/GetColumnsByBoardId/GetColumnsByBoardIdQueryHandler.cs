using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.Columns.Queries.GetColumnsByBoardId;

public record GetColumnsByBoardId(Guid UserId, Guid BoardId) : IRequest<Result<List<ColumnDto>>>;

public class GetColumnsByBoardIdHandler : IRequestHandler<GetColumnsByBoardId, Result<List<ColumnDto>>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;

    public GetColumnsByBoardIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<ColumnDto>>> Handle(GetColumnsByBoardId request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<List<ColumnDto>>.Failure(new UnauthorizedAccessException());

        var board = await _context.Boards
            .Include(b => b.UserBoards)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.UserTasks)
            .ThenInclude(ut => ut.User)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.TaskActivityLogs)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken: cancellationToken);

        if (board == null) return Result<List<ColumnDto>>.Failure(new NotFoundException("Board not found"));
        if (!board.UserBoards.Any(ub => ub.UserId == user.Id)) return Result<List<ColumnDto>>.Failure(new ForbiddenException());

        var columns = board.Columns.OrderBy(c => c.Order).ToList();

        columns.ForEach(cd => cd.Tasks.ForEach(t => t.TaskActivityLogs = t.TaskActivityLogs.OrderByDescending(l => l.CreatedAt).ToList()));

        columns.ForEach(cd => cd.Tasks = cd.Tasks.OrderBy(t => t.DueDate)
                            .ThenByDescending(t => t.Priority)
                            .ThenBy(t => t.CreatedAt)
                            .ToList());

        var columnsDto = _mapper.Map<List<ColumnDto>>(columns);

        return Result<List<ColumnDto>>.Success(columnsDto);
    }
}