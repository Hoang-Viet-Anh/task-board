using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Boards.Queries.GetBoardById;

public record GetBoardByIdQuery(Guid UserId, Guid BoardId) : IRequest<BoardDto>;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDto>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;
    public GetBoardByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var board = await _context.Boards
            .Include(b => b.Columns)
            .ThenInclude(c => c.Tasks)
            .ThenInclude(t => t.UserTasks)
            .ThenInclude(ut => ut.User)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken: cancellationToken) ?? throw new NotFoundException("Board not found.");
        var isMemberOfBoard = await _context.UserBoards.FirstOrDefaultAsync(ub => ub.UserId == user.Id && ub.BoardId == board.Id, cancellationToken: cancellationToken) ?? throw new ForbiddenException();
        var isOwnerOfBoard = user.Id == board.OwnerId;

        var boardDto = _mapper.Map<BoardDto>(board);

        boardDto.IsOwner = isOwnerOfBoard;
        if (!isOwnerOfBoard)
        {
            boardDto.InviteCode = null;
        }

        return boardDto;
    }
}