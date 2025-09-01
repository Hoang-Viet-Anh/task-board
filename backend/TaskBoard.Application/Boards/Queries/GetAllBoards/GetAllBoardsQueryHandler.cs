using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Boards.Queries.GetAllBoards;

public record GetAllBoardsQuery(Guid UserId) : IRequest<List<BoardDto>>;

public class GetAllBoardsQueryHandler : IRequestHandler<GetAllBoardsQuery, List<BoardDto>>
{
    public readonly IApplicationDbContext _context;
    public readonly IMapper _mapper;

    public GetAllBoardsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BoardDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken) ?? throw new UnauthorizedAccessException();
        var userBoards = await _context.UserBoards
            .Where(ub => ub.UserId == user.Id)
            .Include(ub => ub.Board)
            .Select(ub => ub.Board)
            .OrderBy(ub => ub.CreatedAt)
            .ToListAsync(cancellationToken: cancellationToken);

        var userBoardsDto = _mapper.Map<List<BoardDto>>(userBoards);

        userBoardsDto.ForEach(b =>
        {
            b.InviteCode = null;
            b.IsOwner = b.OwnerId == user.Id;
        });

        return userBoardsDto;
    }
}