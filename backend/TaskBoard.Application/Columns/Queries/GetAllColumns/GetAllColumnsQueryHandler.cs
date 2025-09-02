using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Columns.Queries.GetAllColumns;

public record GetAllColumnsQuery(Guid UserId, Guid BoardId) : IRequest<List<ColumnDto>>;

public class GetAllColumnsQueryHandler : IRequestHandler<GetAllColumnsQuery, List<ColumnDto>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;

    public GetAllColumnsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ColumnDto>> Handle(GetAllColumnsQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var board = await _context.Boards
            .Include(b => b.UserBoards)
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.UserTasks)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Board not found");

        if (!board.UserBoards.Any(ub => ub.UserId == user.Id)) throw new ForbiddenException();

        var columnsDto = _mapper.Map<List<ColumnDto>>(board.Columns);

        return columnsDto;
    }
}