using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Boards.Commands.LeaveBoard;
using TaskBoard.Application.Common.Interfaces;

namespace UnitTests.Boards;

public class LeaveBoardCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    public LeaveBoardCommandHandlerTests()
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
    public async Task LeaveBoardWithCorrectParams()
    {
        //Arrange
        var command = new LeaveBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var handler = new LeaveBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task LeaveBoardWithNonExistUserId()
    {
        //Arrange
        var command = new LeaveBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111117"), Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var handler = new LeaveBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task LeaveBoardWithNonExistBoardId()
    {
        //Arrange
        var command = new LeaveBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("32222222-2222-2222-2222-222222222222"));
        var handler = new LeaveBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }
}
