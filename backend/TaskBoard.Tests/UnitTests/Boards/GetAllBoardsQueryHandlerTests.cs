using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Boards.Queries.GetAllBoards;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace UnitTests.Boards;

public class GetAllBoardQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly IMapper _mapper;

    public GetAllBoardQueryHandlerTests( )
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
    public async System.Threading.Tasks.Task GetAllBoardsWithCorrectUserId()
    {
        //Arrange
        var command = new GetAllBoardsQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        var handler = new GetAllBoardsQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllBoardsWithNonExistUserId()
    {
        //Arrange
        var command = new GetAllBoardsQuery(Guid.Parse("11111111-1111-1111-1111-111111111119"));
        var handler = new GetAllBoardsQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }
}
