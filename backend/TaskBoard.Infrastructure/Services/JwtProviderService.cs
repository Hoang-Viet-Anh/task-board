using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Services;

public class JwtProviderService : IJwtProviderService
{
    public readonly IConfiguration _configuration;
    public readonly IApplicationDbContext _context;
    public const int RefreshTokenExpirationInDays = 1;
    public const int AccessTokenExpirationInMinutes = 5;
    public const string AccessTokenKey = "access_token";
    public const string RefreshTokenKey = "refresh_token";

    public JwtProviderService(IConfiguration configuration, IApplicationDbContext context)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<TokenResponse> Create(User user, CancellationToken cancellationToken)
    {
        string secretKey = _configuration["Jwt:Secret"] ?? throw new Exception("Jwt secret not found.");
        string issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("Issuer not found");
        string audience = _configuration["Jwt:Audience"] ?? throw new Exception("Audience is not found");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(AccessTokenExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        string accessToken = handler.WriteToken(token);
        string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var hashedRefreshToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

        _context.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = hashedRefreshToken,
            UserId = user.Id,
            User = user,
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationInDays)
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new TokenResponse(accessToken, refreshToken);
    }

    public void SetJwtCookies(TokenResponse tokens, HttpResponse response)
    {
        response.Cookies.Append(AccessTokenKey, tokens.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(AccessTokenExpirationInMinutes)
        });

        response.Cookies.Append(RefreshTokenKey, tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(RefreshTokenExpirationInDays)
        });
    }

    public void ClearJwtCookies(HttpResponse response)
    {
        response.Cookies.Delete(AccessTokenKey);
        response.Cookies.Delete(RefreshTokenKey);
    }
}