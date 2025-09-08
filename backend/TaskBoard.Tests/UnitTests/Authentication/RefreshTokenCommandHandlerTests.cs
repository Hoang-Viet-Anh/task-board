using FluentAssertions;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;


namespace UnitTests.Authentication;

public class RefreshTokenCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly Mock<IJwtProviderService> _jwtProviderService;

    public RefreshTokenCommandHandlerTests()
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;

        _jwtProviderService = new();

        _jwtProviderService.Setup(jps => jps.Create(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TokenResponse("Access Token", "Refresh Token"));
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public async System.Threading.Tasks.Task RefreshExistingToken()
    {
        //Arrange
        var command = new RefreshTokenCommand("Token Hash 1");
        var handler = new RefreshTokenCommandHandler(_context, _jwtProviderService.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task RefreshNonExistingToken()
    {
        //Arrange
        var command = new RefreshTokenCommand("Token Hash 2");
        var handler = new RefreshTokenCommandHandler(_context, _jwtProviderService.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task RefreshEmptyToken()
    {
        //Arrange
        var command = new RefreshTokenCommand("");
        var handler = new RefreshTokenCommandHandler(_context, _jwtProviderService.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }
}
