using Microsoft.Extensions.Configuration;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Services;

public class JwtProviderService
{
    public readonly IConfiguration _configuration;

    public JwtProviderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Create(User user)
    {
        string secretKey = _configuration["Jwt:Secret"] ?? throw new Exception("Jwt secret not found.");
        return "";
    }
}