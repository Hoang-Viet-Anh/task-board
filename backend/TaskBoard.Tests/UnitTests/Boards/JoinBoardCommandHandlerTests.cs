using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Boards.Commands.JoinBoard;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace UnitTests.Boards;

public class JoinBoardCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    public JoinBoardCommandHandlerTests()
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
    public async Task JoinBoardWithCorrectParams()
    {
        //Arrange
        var command = new JoinBoardCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), "code");
        var handler = new JoinBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task JoinBoardWithWrongCode()
    {
        //Arrange
        var command = new JoinBoardCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), "code1");
        var handler = new JoinBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task JoinBoardWithWrongUserId()
    {
        //Arrange
        var command = new JoinBoardCommand(Guid.Parse("91111111-1111-1111-1111-111111111111"), "code");
        var handler = new JoinBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task JoinBoardWithBoardMemberId()
    {
        //Arrange
        var command = new JoinBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), "code");
        var handler = new JoinBoardCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }
}
