using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Boards.Queries.GetBoardById;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace UnitTests.Boards;

public class GetBoardByIdQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly IMapper _mapper;

    public GetBoardByIdQueryHandlerTests( )
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
    public async System.Threading.Tasks.Task GetBoardByIdWithCorrectParams()
    {
        //Arrange
        var command = new GetBoardByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var handler = new GetBoardByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetBoardByIdWithNonExistUserId()
    {
        //Arrange
        var command = new GetBoardByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111119"), Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var handler = new GetBoardByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetBoardByIdWithNonExistBoardId()
    {
        //Arrange
        var command = new GetBoardByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("32222222-2222-2222-2222-222222222222"));
        var handler = new GetBoardByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundException>();
    }


    [Fact]
    public async System.Threading.Tasks.Task GetBoardByIdWithNonRelatedUser()
    {
        //Arrange
        var command = new GetBoardByIdQuery(Guid.Parse("21111111-1111-1111-1111-111111111111"), Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var handler = new GetBoardByIdQueryHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ForbiddenException>();
    }
}
