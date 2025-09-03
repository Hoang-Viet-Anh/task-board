using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Tasks.Commands.CreateTask;
using TaskBoard.Application.Tasks.Commands.DeleteTask;
using TaskBoard.Application.Tasks.Commands.MoveTask;
using TaskBoard.Application.Tasks.Commands.UpdateTask;
using TaskBoard.Application.Tasks.Queries.GetTaskById;

namespace TaskBoard.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public TaskController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        var userId = _currentUserService.GetUserId();

        var command = new CreateTaskCommand(userId, taskDto);
        await _mediator.Send(command);

        return Created();
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateTask([FromBody] TaskDto TaskDto)
    {
        var userId = _currentUserService.GetUserId();

        var command = new UpdateTaskCommand(userId, TaskDto);
        await _mediator.Send(command);

        return Ok();
    }

    [HttpPost("move")]
    public async Task<IActionResult> MoveTask([FromBody] TaskDto TaskDto)
    {
        var userId = _currentUserService.GetUserId();

        var command = new MoveTaskCommand(userId, TaskDto);
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("{TaskId}")]
    public async Task<IActionResult> GetTaskById(Guid TaskId)
    {
        var userId = _currentUserService.GetUserId();

        var query = new GetTaskByIdQuery(userId, TaskId);
        var TaskDto = await _mediator.Send(query);

        return Ok();
    }

    [HttpDelete("delete/{TaskId}")]
    public async Task<IActionResult> DeleteTask(Guid TaskId)
    {
        var userId = _currentUserService.GetUserId();

        var command = new DeleteTaskCommand(userId, TaskId);
        await _mediator.Send(command);

        return Ok();
    }

}