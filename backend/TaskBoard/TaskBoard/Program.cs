using DotNetEnv;
using TaskBoard.Application;
using TaskBoard.Infrastructure;
using TaskBoard;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
