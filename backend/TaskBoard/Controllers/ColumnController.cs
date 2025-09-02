using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Columns.Commands.CreateColumn;
using TaskBoard.Application.Columns.Commands.DeleteColumn;
using TaskBoard.Application.Columns.Commands.UpdateColumn;
using TaskBoard.Application.Columns.Queries.GetAllColumns;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ColumnController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public ColumnController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateColumn([FromBody] ColumnDto ColumnDto)
    {
        var userId = _currentUserService.GetUserId();

        var command = new CreateColumnCommand(userId, ColumnDto);
        await _mediator.Send(command);

        return Created();
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateColumn([FromBody] ColumnDto ColumnDto)
    {
        var userId = _currentUserService.GetUserId();

        var command = new UpdateColumnCommand(userId, ColumnDto);
        await _mediator.Send(command);

        return Ok();
    }

    [HttpDelete("delete/{ColumnId}")]
    public async Task<IActionResult> DeleteColumn(Guid ColumnId)
    {
        var userId = _currentUserService.GetUserId();

        var command = new DeleteColumnCommand(userId, ColumnId);
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("get-all/{BoardId}")]
    public async Task<IActionResult> GetAllColumnsByBoardId(Guid BoardId)
    {
        var userId = _currentUserService.GetUserId();

        var query = new GetAllColumnsQuery(userId, BoardId);
        var columns = await _mediator.Send(query);

        return Ok(columns);
    }

}