using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Columns.Commands.UpdateColumn;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace UnitTests.Columns;

public class UpdateColumnCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    public UpdateColumnCommandHandlerTests()
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
    public async Task UpdateColumnWithCorrectParams()
    {
        //Arrange
        var command = new UpdateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Title 4",
            Order = "z"
        });
        var handler = new UpdateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateColumnWithNonExistUserId()
    {
        //Arrange
        var command = new UpdateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), new ColumnDto
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Title 4",
            Order = "z"
        });
        var handler = new UpdateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task UpdateColumnWithNonExistColumnId()
    {
        //Arrange
        var command = new UpdateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            Id = Guid.Parse("43333333-3333-3333-3333-333333333333"),
            Title = "New Title 4",
            Order = "z"
        });
        var handler = new UpdateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task UpdateColumnWithEmptyParams()
    {
        //Arrange
        var command = new UpdateColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "",
            Order = ""
        });
        var handler = new UpdateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }

    [Fact]
    public async Task UpdateColumnWithNonRelatedUser()
    {
        //Arrange
        var command = new UpdateColumnCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), new ColumnDto
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Title",
            Order = "z"
        });
        var handler = new UpdateColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }
}
