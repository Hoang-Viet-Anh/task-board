using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Application.Tasks.Commands.DeleteTask;

namespace UnitTests.Tasks;

public class DeleteTaskCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<IMediator> _mediator;

    public DeleteTaskCommandHandlerTests()
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;
        _configuration = new();
        _mediator = new();

        _mediator.Setup(m => m.Send(It.IsAny<IRequest<Result<Unit>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<Unit>.Success(Unit.Value));
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public async Task DeleteTaskWithCorrectParams()
    {
        //Arrange
        var command = new DeleteTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var handler = new DeleteTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task DeleteTaskWithNonExistUserId()
    {
        //Arrange
        var command = new DeleteTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var handler = new DeleteTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteTaskWithNonExistTaskId()
    {
        //Arrange
        var command = new DeleteTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("44444444-4444-4444-4444-444444444449"));
        var handler = new DeleteTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaskWithNonRelatedUserId()
    {
        //Arrange
        var command = new DeleteTaskCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var handler = new DeleteTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }
}
