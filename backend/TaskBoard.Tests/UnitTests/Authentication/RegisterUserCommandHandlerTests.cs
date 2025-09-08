using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Moq;
using TaskBoard.Application.Authentication.Commands.Register;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;


namespace UnitTests.Authentication;

public class RegisterUserCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly Mock<IPasswordHasher<User>> _passwordHasher;

    public RegisterUserCommandHandlerTests()
    {
        var (context, connection) = Mockdata.CreateMockDbContext();
        _context = context;
        _connection = connection;

        _passwordHasher = new();

        _passwordHasher.Setup(pw => pw.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("Hash");
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Fact]
    public async System.Threading.Tasks.Task UserRegistration()
    {
        //Arrange
        var command = new RegistrationUserCommand("User 4", "Hash 4");
        var handler = new RegistrationUserCommandHandler(_context, _passwordHasher.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task UserAlreadyExist()
    {
        //Arrange
        var command = new RegistrationUserCommand("User 1", "Hash 1");
        var handler = new RegistrationUserCommandHandler(_context, _passwordHasher.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<ConflictException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task EmptyUserData()
    {
        //Arrange
        var command = new RegistrationUserCommand("", "");
        var handler = new RegistrationUserCommandHandler(_context, _passwordHasher.Object);

        //Act
        var result = await handler.Handle(command, default);

        //Assertion
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<BadRequestException>();
    }
}
