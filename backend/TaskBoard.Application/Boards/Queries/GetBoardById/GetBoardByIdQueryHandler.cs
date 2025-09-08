using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;

namespace TaskBoard.Application.Boards.Queries.GetBoardById;

public record GetBoardByIdQuery(Guid UserId, Guid BoardId) : IRequest<Result<BoardDto>>;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, Result<BoardDto>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;
    public GetBoardByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<BoardDto>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

        if (user == null) return Result<BoardDto>.Failure(new UnauthorizedAccessException());

        var board = await _context.Boards
            .Include(b => b.UserBoards)
            .ThenInclude(ub => ub.User)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken: cancellationToken);

        if (board == null) return Result<BoardDto>.Failure(new NotFoundException("Board not found."));

        var isMemberOfBoard = await _context.UserBoards.FirstOrDefaultAsync(ub => ub.UserId == user.Id && ub.BoardId == board.Id, cancellationToken: cancellationToken);

        if (isMemberOfBoard == null) return Result<BoardDto>.Failure(new ForbiddenException());

        var isOwnerOfBoard = user.Id == board.OwnerId;

        var boardDto = _mapper.Map<BoardDto>(board);

        boardDto.IsOwner = isOwnerOfBoard;
        if (!isOwnerOfBoard)
        {
            boardDto.InviteCode = null;
        }

        return Result<BoardDto>.Success(boardDto);
    }
}