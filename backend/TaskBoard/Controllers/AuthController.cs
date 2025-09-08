using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sprache;
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
        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        _jwtProviderService.SetJwtCookies(result.Value, Response);

        return Ok();
    }

    [HttpPost("refresh-tokens")]
    public async Task<IActionResult> RefreshTokens()
    {
        _jwtProviderService.ClearJwtCookies(Response);

        var refreshToken = Request.Cookies[JwtProviderService.RefreshTokenKey] ?? throw new UnauthorizedAccessException();

        var command = new RefreshTokenCommand(refreshToken);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess) throw result.Error;

        _jwtProviderService.SetJwtCookies(result.Value, Response);

        return Ok();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _jwtProviderService.ClearJwtCookies(Response);
        return Ok();
    }
}