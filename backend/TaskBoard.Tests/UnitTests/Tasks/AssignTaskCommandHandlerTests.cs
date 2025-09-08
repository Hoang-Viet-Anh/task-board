using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Application.Tasks.Commands.AssignTask;

namespace UnitTests.Tasks;

public class AssignTaskCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<IMediator> _mediator;

    public AssignTaskCommandHandlerTests()
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
    public async Task AssignTaskWithCorrectParams()
    {
        //Arrange
        var command = new AssignTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new AssignDto
        {
            TaskId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AsigneeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        });
        var handler = new AssignTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AssignTaskWithWrongAssigneeId()
    {
        //Arrange
        var command = new AssignTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new AssignDto
        {
            TaskId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AsigneeId = Guid.Parse("11111111-1111-1111-1111-111111111119")
        });
        var handler = new AssignTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task AssignTaskWithWrongTaskId()
    {
        //Arrange
        var command = new AssignTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new AssignDto
        {
            TaskId = Guid.Parse("44444444-4444-4444-4444-444444444449"),
            AsigneeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        });
        var handler = new AssignTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task AssignTaskWithWrongUserId()
    {
        //Arrange
        var command = new AssignTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), new AssignDto
        {
            TaskId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AsigneeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        });
        var handler = new AssignTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task AssignTaskWithNonRelatedUserId()
    {
        //Arrange
        var command = new AssignTaskCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), new AssignDto
        {
            TaskId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AsigneeId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        });
        var handler = new AssignTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }


    [Fact]
    public async Task AssignTaskWithNonRelatedAssigneeId()
    {
        //Arrange
        var command = new AssignTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new AssignDto
        {
            TaskId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            AsigneeId = Guid.Parse("21111111-1111-1111-1111-111111111111")
        });
        var handler = new AssignTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }
}
