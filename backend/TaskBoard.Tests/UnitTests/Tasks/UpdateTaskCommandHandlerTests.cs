using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Application.Tasks.Commands.UpdateTask;

namespace UnitTests.Tasks;

public class UpdateTaskCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<IMediator> _mediator;

    public UpdateTaskCommandHandlerTests()
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
    public async Task UpdateTaskWithCorrectParams()
    {
        //Arrange
        var command = new UpdateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new TaskDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task 2",
            Description = "desc 2",
            DueDate = DateTime.Now,
            Priority = "medium"
        });
        var handler = new UpdateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTaskWithNonExistUserId()
    {
        //Arrange
        var command = new UpdateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), new TaskDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task 2",
            Description = "desc 2",
            DueDate = DateTime.Now,
            Priority = "medium"
        });
        var handler = new UpdateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task UpdateTaskWithNonExistTaskId()
    {
        //Arrange
        var command = new UpdateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new TaskDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444449"),
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task 2",
            Description = "desc 2",
            DueDate = DateTime.Now,
            Priority = "medium"
        });
        var handler = new UpdateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task UpdateTaskWithNonExistColumnId()
    {
        //Arrange
        var command = new UpdateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new TaskDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333339"),
            Title = "New Task 2",
            Description = "desc 2",
            DueDate = DateTime.Now,
            Priority = "medium"
        });
        var handler = new UpdateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }


    [Fact]
    public async Task UpdateTaskWithNonRelatedUser()
    {
        //Arrange
        var command = new UpdateTaskCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), new TaskDto
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task 2",
            Description = "desc 2",
            DueDate = DateTime.Now,
            Priority = "medium"
        });
        var handler = new UpdateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }
}
