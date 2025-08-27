using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Services;

public class JwtProviderService : IJwtProviderService
{
    public readonly IConfiguration _configuration;
    public const int JwtExpirationInMinutes = 5;

    public JwtProviderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Create(User user)
    {
        string secretKey = _configuration["Jwt:Secret"] ?? throw new Exception("Jwt secret not found.");
        string issuer = _configuration["Jwt:Issuer"] ?? throw new Exception("Issuer not found");
        string audience = _configuration["Jwt:Audience"] ?? throw new Exception("Audience is not found");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(JwtExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}