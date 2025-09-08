using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Application.Tasks.Queries.GetTaskById;

namespace UnitTests.Tasks;

public class GetTaskByIdQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;

    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandlerTests( )
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;
        _mapper = Mockdata.CreateProfiledMapper();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public async Task GetTaskByIdWithCorrectParams()
    {
        //Arrange
        var command = new GetTaskByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var handler = new GetTaskByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetTaskByIdWithNonExistUserId()
    {
        //Arrange
        var command = new GetTaskByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111119"), Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var handler = new GetTaskByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetTaskByIdWithNonExistTaskId()
    {
        //Arrange
        var command = new GetTaskByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("44444444-4444-4444-4444-444444444449"));
        var handler = new GetTaskByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task GetTaskByIdWithNonRelatedUserId()
    {
        //Arrange
        var command = new GetTaskByIdQuery(Guid.Parse("21111111-1111-1111-1111-111111111111"), Guid.Parse("44444444-4444-4444-4444-444444444444"));
        var handler = new GetTaskByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }
}
