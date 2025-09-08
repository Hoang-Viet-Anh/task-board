using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Application.Tasks.Commands.CreateTask;
using TaskBoard.Domain.Enums;

namespace UnitTests.Tasks;

public class CreateTaskCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<IMediator> _mediator;

    public CreateTaskCommandHandlerTests()
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
    public async Task CreateTaskWithCorrectParams()
    {
        //Arrange
        var command = new CreateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), new TaskDto
        {
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task",
            Description = "desc",
            DueDate = DateTime.Now,
            Priority = "low"
        });
        var handler = new CreateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task CreateTaskWithNonExistUser()
    {
        //Arrange
        var command = new CreateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), new TaskDto
        {
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task",
            Description = "desc",
            DueDate = DateTime.Now,
            Priority = "low"
        });
        var handler = new CreateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task CreateTaskWithNonExistColumnId()
    {
        //Arrange
        var command = new CreateTaskCommand(Guid.Parse("11111111-1111-1111-1111-111111111119"), new TaskDto
        {
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
            Title = "New Task",
            Description = "desc",
            DueDate = DateTime.Now,
            Priority = "low"
        });
        var handler = new CreateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task CreateTaskWithNonRelatedUserId()
    {
        //Arrange
        var command = new CreateTaskCommand(Guid.Parse("21111111-1111-1111-1111-111111111111"), new TaskDto
        {
            ColumnId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "New Task",
            Description = "desc",
            DueDate = DateTime.Now,
            Priority = "low"
        });
        var handler = new CreateTaskCommandHandler(_context, _mediator.Object, _configuration.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }
}
