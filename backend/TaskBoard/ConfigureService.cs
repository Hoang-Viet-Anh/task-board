using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Filters;
using TaskBoard.Services;

namespace TaskBoard;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            });
        });

        services.AddMediatR(typeof(Program).Assembly);

        services.AddControllers(opts => opts.Filters.Add<ApiExceptionFilterAttribute>())
            .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
        {
            var jwtSecret = configuration["Jwt:Secret"] ?? throw new NotFoundException("Secret not found");
            var issuer = configuration["Jwt:Issuer"] ?? throw new NotFoundException("Issuer not found");
            var audience = configuration["Jwt:Audience"] ?? throw new NotFoundException("Audience not found");

            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidIssuer = issuer,
                ValidAudience = audience,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}