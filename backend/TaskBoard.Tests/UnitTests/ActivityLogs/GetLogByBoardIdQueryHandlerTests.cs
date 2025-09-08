using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.ActivityLogs.Queries.GetLogByBoardId;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;


namespace UnitTests.ActivityLogs;

public class GetLogByBoardIdQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly IMapper _mapper;

    public GetLogByBoardIdQueryHandlerTests( )
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;
        _mapper = Mockdata.CreateProfiledMapper();
    }

    [Fact]
    public async Task GetLogsWithCorrectParams()
    {
        //Arrange
        var command = new GetLogByBoardIdQuery(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            0
        );
        var handler = new GetLogByBoardIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetLogsWithNonExistingUser()
    {
        //Arrange
        var command = new GetLogByBoardIdQuery(
            Guid.Parse("11111111-1111-1111-1111-111111111115"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            0
        );
        var handler = new GetLogByBoardIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetLogsWithNonExistingBoard()
    {
        //Arrange
        var command = new GetLogByBoardIdQuery(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("32222222-2222-2222-2222-222222222222"),
            0
        );
        var handler = new GetLogByBoardIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }

    [Fact]
    public async Task GetLogsFromAnUnauthorizedUser()
    {
        //Arrange
        var command = new GetLogByBoardIdQuery(
            Guid.Parse("21111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            0
        );
        var handler = new GetLogByBoardIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
