using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Mapping;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskBoard.Infrastructure.Persistence;

namespace UnitTests;

public class Mockdata
{
    public static (ApplicationDbContext, SqliteConnection) CreateMockDbContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(contextOptions);
        context.Database.EnsureCreated();

        SeedingDb(context);

        return (context, connection);
    }

    public static IMapper CreateProfiledMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new EntityMappingProfiles());
        });

        config.AssertConfigurationIsValid();

        return config.CreateMapper();
    }

    public static void SeedingDb(ApplicationDbContext context)
    {
        var user1 = new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Username = "User 1",
            PasswordHash = "Hash 1"
        };
        var user2 = new User
        {
            Id = Guid.Parse("21111111-1111-1111-1111-111111111111"),
            Username = "User 2",
            PasswordHash = "Hash 2"
        };
        context.Users.AddRange(user1, user2);

        var board1 = new Board
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "Board 1",
            InviteCode = "code",
            OwnerId = user1.Id,
            Owner = user1
        };
        context.Boards.Add(board1);

        var userBoard1 = new UserBoard
        {
            UserId = user1.Id,
            BoardId = board1.Id,
            User = user1,
            Board = board1
        };
        context.UserBoards.Add(userBoard1);

        board1.UserBoards = new List<UserBoard> { userBoard1 };

        var column1 = new Column
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            BoardId = board1.Id,
            Board = board1,
            Title = "New List",
            Order = "a"
        };
        context.Columns.Add(column1);

        var column2 = new Column
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333334"),
            BoardId = board1.Id,
            Board = board1,
            Title = "New List 2",
            Order = "b"
        };
        context.Columns.Add(column2);

        var task1 = new TaskBoard.Domain.Entities.Task
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Title = "Task Title",
            DueDate = DateTime.Now,
            Priority = TaskPriority.LOW,
            ColumnId = column1.Id,
            Column = column1
        };
        context.Tasks.Add(task1);

        var userTask1 = new UserTask
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            UserId = user1.Id,
            TaskId = task1.Id,
            User = user1,
            Task = task1
        };
        context.UserTasks.Add(userTask1);

        var token1 = new RefreshToken
        {
            UserId = user1.Id,
            TokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("Token Hash 1"))),
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        context.RefreshTokens.Add(token1);

        var log1 = new TaskActivityLog
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            BoardId = board1.Id,
            UserId = user1.Id,
            Board = board1,
            User = user1,
            Log = "Test Log"
        };
        context.TaskActivityLogs.Add(log1);

        context.SaveChanges();
    }

}