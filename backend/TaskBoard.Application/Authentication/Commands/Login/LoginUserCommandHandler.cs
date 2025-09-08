using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Authentication.Commands.Login;

public record LoginUserCommand(string Username, string Password) : IRequest<Result<TokenResponse>>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<TokenResponse>>
{
    public readonly IApplicationDbContext _context;
    public readonly IJwtProviderService _jwtProviderService;
    public readonly IPasswordHasher<User> _passwordHasher;

    public LoginUserCommandHandler(IApplicationDbContext context, IJwtProviderService jwtProviderService, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _jwtProviderService = jwtProviderService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username.Trim()) || string.IsNullOrEmpty(request.Password)) return Result<TokenResponse>.Failure(new BadRequestException("Credentials is required"));

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken: cancellationToken);

        if (user == null) return Result<TokenResponse>.Failure(new UnauthorizedAccessException());

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);

        if (result == PasswordVerificationResult.Failed) return Result<TokenResponse>.Failure(new UnauthorizedAccessException());

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        }

        var tokenResponse = await _jwtProviderService.Create(user, cancellationToken);

        return Result<TokenResponse>.Success(tokenResponse);
    }
}