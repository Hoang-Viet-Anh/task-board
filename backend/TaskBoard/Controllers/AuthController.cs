using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Authentication.Commands;

namespace TaskBoard.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationUserCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}