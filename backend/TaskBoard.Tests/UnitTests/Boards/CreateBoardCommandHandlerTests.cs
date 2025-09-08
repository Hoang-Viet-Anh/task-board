using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Boards.Commands.CreateBoard;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Infrastructure.Services;

namespace UnitTests.Boards;

public class CreateBoardCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly IInviteCodeGenerator _codeGenerator;

    public CreateBoardCommandHandlerTests()
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;

        _codeGenerator = new InviteCodeGenerator(_context);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public async Task CorrectBoardCretion()
    {
        //Arrange
        var command = new CreateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), "New Board");
        var handler = new CreateBoardCommandHandler(_context, _codeGenerator);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task BoardCreationWithNonExistingUser()
    {
        //Arrange
        var command = new CreateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111113"), "New Board");
        var handler = new CreateBoardCommandHandler(_context, _codeGenerator);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task BoardCreationWithEmptyTitle()
    {
        //Arrange
        var command = new CreateBoardCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), "");
        var handler = new CreateBoardCommandHandler(_context, _codeGenerator);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }
}
