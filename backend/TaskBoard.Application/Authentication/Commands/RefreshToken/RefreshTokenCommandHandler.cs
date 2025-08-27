using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponse>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    public readonly IApplicationDbContext _context;
    public readonly IJwtProviderService _jwtProviderService;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtProviderService jwtProviderService)
    {
        _context = context;
        _jwtProviderService = jwtProviderService;
    }

    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hashedRefreshToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken)));

        var refreshTokenRecord = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == hashedRefreshToken && rt.ExpiresAt > DateTime.UtcNow, cancellationToken: cancellationToken)
            ?? throw new UnauthorizedAccessException();

        _context.RefreshTokens.Remove(refreshTokenRecord);

        var tokenResponse = await _jwtProviderService.Create(refreshTokenRecord.User, cancellationToken);

        return tokenResponse;
    }
}