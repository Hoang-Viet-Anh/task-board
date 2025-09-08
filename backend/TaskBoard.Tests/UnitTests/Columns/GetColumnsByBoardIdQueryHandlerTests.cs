using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Columns.Queries.GetColumnsByBoardId;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;

namespace UnitTests.Columns;

public class GetColumnsByBoardIdQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly IMapper _mapper;


    public GetColumnsByBoardIdQueryHandlerTests()
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
    public async System.Threading.Tasks.Task GetColumnByBoardIdWithCorrectParams()
    {
        //Arrange
        var command = new GetColumnsByBoardId(Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("22222222-2222-2222-2222-222222222222"));
        var handler = new GetColumnsByBoardIdHandler(_context, _mapper);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }
}
