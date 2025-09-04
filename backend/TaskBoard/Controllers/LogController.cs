using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.ActivityLogs.Queries.GetLogByBoardId;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LogController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public LogController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet("{BoardId}/{Page}")]
    public async Task<IActionResult> GetAllColumnsByBoardId(Guid BoardId, int Page)
    {
        var userId = _currentUserService.GetUserId();

        var query = new GetLogByBoardIdQuery(userId, BoardId, Page);
        var columns = await _mediator.Send(query);

        return Ok(columns);
    }

}