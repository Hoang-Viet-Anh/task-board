using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Authentication.Commands.Login;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;


namespace UnitTests.Authentication;

public class LoginUserCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly Mock<IJwtProviderService> _jwtProviderService;
    private readonly Mock<IPasswordHasher<User>> _passwordHasher;

    public LoginUserCommandHandlerTests()
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;

        _jwtProviderService = new();
        _passwordHasher = new();

        _jwtProviderService.Setup(jps => jps.Create(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TokenResponse("Access Token", "Refresh Token"));
        _passwordHasher.Setup(pw => pw.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("Hash");
    }

    [Fact]
    public async System.Threading.Tasks.Task LoginCorrectCredentials()
    {
        //Arrange
        var command = new LoginUserCommand("User 1", "Hash 1");

        _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);

        var handler = new LoginUserCommandHandler(_context, _jwtProviderService.Object, _passwordHasher.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task LoginNonExistingUser()
    {
        //Arrange
        var command = new LoginUserCommand("User 5", "Hash 5");
        _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
        var handler = new LoginUserCommandHandler(_context, _jwtProviderService.Object, _passwordHasher.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task LoginWrongCredentials()
    {
        //Arrange
        var command = new LoginUserCommand("User 1", "Hash 2");
        _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Failed);
        var handler = new LoginUserCommandHandler(_context, _jwtProviderService.Object, _passwordHasher.Object);
        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UnauthorizedAccessException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task EmptyCredentials()
    {
        //Arrange
        var command = new LoginUserCommand("", "");
        _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
        var handler = new LoginUserCommandHandler(_context, _jwtProviderService.Object, _passwordHasher.Object);
        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
