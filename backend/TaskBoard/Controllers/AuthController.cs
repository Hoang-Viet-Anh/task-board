using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Authentication.Commands.Login;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Application.Authentication.Commands.Register;

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
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var tokenResponse = await _mediator.Send(command);
        return Ok(tokenResponse);
    }

    [HttpPost("refresh-tokens")]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokenCommand command)
    {
        var tokenResponse = await _mediator.Send(command);
        return Ok(tokenResponse);
    }
}