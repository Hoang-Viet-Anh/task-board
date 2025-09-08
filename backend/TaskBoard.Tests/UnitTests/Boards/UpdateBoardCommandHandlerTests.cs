using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Boards.Commands.UpdateBoard;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace UnitTests.Boards;

public class UpdateBoardCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;



    public UpdateBoardCommandHandlerTests()
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public async Task UpdateBoardWithCorrectParams()
    {
        //Arrange
        var command = new UpdateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"),
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "New Title 2"
        });
        var handler = new UpdateBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateBoardWithNonExistUserId()
    {
        //Arrange
        var command = new UpdateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"),
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "New Title 2"
        });
        var handler = new UpdateBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task UpdateBoardWithNonOwner()
    {
        //Arrange
        var command = new UpdateBoardCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"),
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "New Title 2"
        });
        var handler = new UpdateBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }

    [Fact]
    public async Task UpdateBoardWithNonExistBoardId()
    {
        //Arrange
        var command = new UpdateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"),
        new()
        {
            Id = Guid.Parse("32222222-2222-2222-2222-222222222222"),
            Title = "New Title 2"
        });
        var handler = new UpdateBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task UpdateBoardWithEmptyBoardTitle()
    {
        //Arrange
        var command = new UpdateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"),
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = ""
        });
        var handler = new UpdateBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }
}
