using DotNetEnv;
using TaskBoard.Application;
using TaskBoard.Infrastructure;
using TaskBoard;
using TaskBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (db.Database.CanConnect())
        {
            db.Database.Migrate();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to apply migrations: {e}");
    }
}
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
