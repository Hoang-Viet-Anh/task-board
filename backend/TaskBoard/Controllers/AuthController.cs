using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Authentication.Commands.Login;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Application.Authentication.Commands.Register;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Infrastructure.Services;

namespace TaskBoard.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtProviderService _jwtProviderService;
    public AuthController(IMediator mediator, IJwtProviderService jwtProviderService)
    {
        _mediator = mediator;
        _jwtProviderService = jwtProviderService;
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

        _jwtProviderService.SetJwtCookies(tokenResponse, Response);

        return Ok();
    }

    [HttpPost("refresh-tokens")]
    public async Task<IActionResult> RefreshTokens()
    {
        _jwtProviderService.ClearJwtCookies(Response);
        var refreshToken = Request.Cookies[JwtProviderService.RefreshTokenKey] ?? throw new UnauthorizedAccessException();
        var command = new RefreshTokenCommand(refreshToken);
        var tokenResponse = await _mediator.Send(command);
        _jwtProviderService.SetJwtCookies(tokenResponse, Response);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _jwtProviderService.ClearJwtCookies(Response);
        return Ok();
    }
}