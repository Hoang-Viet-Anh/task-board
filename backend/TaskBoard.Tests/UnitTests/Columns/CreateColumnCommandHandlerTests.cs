using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Columns.Commands.CreateColumn;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace UnitTests.Columns;

public class CreateColumnCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;


    public CreateColumnCommandHandlerTests()
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
    public async Task CreateColumnWithCorrectParams()
    {
        //Arrange
        var command = new CreateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "New column",
            Order = "a"
        });
        var handler = new CreateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateColumnWithNonExistUserId()
    {
        //Arrange
        var command = new CreateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), new ColumnDto
        {
            BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "New column",
            Order = "a"
        });
        var handler = new CreateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task CreateColumnWithNonExistBoardId()
    {
        //Arrange
        var command = new CreateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            BoardId = Guid.Parse("32222222-2222-2222-2222-222222222222"),
            Title = "New column",
            Order = "a"
        });
        var handler = new CreateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }


    [Fact]
    public async Task CreateColumnWithNonRelatedBoardId()
    {
        //Arrange
        var command = new CreateColumnCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "New column",
            Order = "a"
        });
        var handler = new CreateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }

    [Fact]
    public async Task CreateColumnWithEmptyColumnTitle()
    {
        //Arrange
        var command = new CreateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            BoardId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "",
            Order = "a"
        });
        var handler = new CreateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }
}
