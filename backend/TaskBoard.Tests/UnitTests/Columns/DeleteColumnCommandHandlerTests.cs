using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Columns.Commands.DeleteColumn;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;

namespace UnitTests.Columns;

public class DeleteColumnCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    public DeleteColumnCommandHandlerTests()
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
    public async Task DeleteColumnWithCorrectParams()
    {
        //Arrange
        var command = new DeleteColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var handler = new DeleteColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteColumnWithNonExistUserId()
    {
        //Arrange
        var command = new DeleteColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var handler = new DeleteColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteColumnWithNonRelatedColumn()
    {
        //Arrange
        var command = new DeleteColumnCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), Guid.Parse("33333333-3333-3333-3333-333333333333"));
        var handler = new DeleteColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }

    [Fact]
    public async Task DeleteColumnWithNonExistColumnId()
    {
        //Arrange
        var command = new DeleteColumnCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("43333333-3333-3333-3333-333333333333"));
        var handler = new DeleteColumnCommandHandler(_context);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }
}
