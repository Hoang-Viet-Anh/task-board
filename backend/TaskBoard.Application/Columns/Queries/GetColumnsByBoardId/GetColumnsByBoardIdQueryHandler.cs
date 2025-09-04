using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Columns.Queries.GetColumnsByBoardId;

public record GetColumnsByBoardId(Guid UserId, Guid BoardId) : IRequest<List<ColumnDto>>;

public class GetColumnsByBoardIdHandler : IRequestHandler<GetColumnsByBoardId, List<ColumnDto>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;

    public GetColumnsByBoardIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ColumnDto>> Handle(GetColumnsByBoardId request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var board = await _context.Boards
            .Include(b => b.UserBoards)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.UserTasks)
            .ThenInclude(ut => ut.User)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.TaskActivityLogs)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Board not found");

        if (!board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        var columns = board.Columns.OrderBy(c => c.CreatedAt).ToList();

        columns.ForEach(cd => cd.Tasks.ForEach(t => t.TaskActivityLogs = t.TaskActivityLogs.OrderByDescending(l => l.CreatedAt).ToList()));

        columns.ForEach(cd => cd.Tasks = cd.Tasks.OrderBy(t => t.DueDate)
                            .ThenByDescending(t => t.Priority)
                            .ThenBy(t => t.CreatedAt)
                            .ToList());

        var columnsDto = _mapper.Map<List<ColumnDto>>(columns);

        return columnsDto;
    }
}