using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Boards.Commands.CreateBoard;
using TaskBoard.Application.Boards.Commands.JoinBoard;
using TaskBoard.Application.Boards.Commands.LeaveBoard;
using TaskBoard.Application.Boards.Commands.UpdateBoard;
using TaskBoard.Application.Boards.Queries.GetAllBoards;
using TaskBoard.Application.Boards.Queries.GetBoardById;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BoardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public BoardController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var userId = _currentUserService.GetUserId();
        var command = new CreateBoardCommand(userId, request.BoardTitle);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        return Created();
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateBoard([FromBody] BoardDto boardDto)
    {
        var userId = _currentUserService.GetUserId();
        var command = new UpdateBoardCommand(userId, boardDto);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        return Ok();
    }

    [HttpGet("{boardId}")]
    public async Task<IActionResult> GetBoardById(Guid boardId)
    {
        var userId = _currentUserService.GetUserId();
        var query = new GetBoardByIdQuery(userId, boardId);

        var result = await _mediator.Send(query);

        if (!result.IsSuccess) throw result.Error;

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBoards()
    {
        var userId = _currentUserService.GetUserId();
        var query = new GetAllBoardsQuery(userId);

        var result = await _mediator.Send(query);

        if (!result.IsSuccess) throw result.Error;

        return Ok(result.Value);
    }

    [HttpDelete("leave/{boardId}")]
    public async Task<IActionResult> LeaveBoard(Guid boardId)
    {
        var userId = _currentUserService.GetUserId();
        var command = new LeaveBoardCommand(userId, boardId);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        return Ok();
    }

    [HttpPost("join/{inviteCode}")]
    public async Task<IActionResult> JoinBoard(string inviteCode)
    {
        var userId = _currentUserService.GetUserId();
        var command = new JoinBoardCommand(userId, inviteCode);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        return Ok();
    }
}