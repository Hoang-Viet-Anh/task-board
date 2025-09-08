using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Application.Common.Result;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Authentication.Commands.Register;

public record RegistrationUserCommand(string Username, string Password) : IRequest<Result<Unit>>;

public class RegistrationUserCommandHandler : IRequestHandler<RegistrationUserCommand, Result<Unit>>
{
    public readonly IApplicationDbContext _context;
    public readonly IPasswordHasher<User> _passwordHasher;

    public RegistrationUserCommandHandler(IApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Unit>> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username.Trim()) || string.IsNullOrEmpty(request.Password)) return Result<Unit>.Failure(new BadRequestException("Username and password is required."));

        var isUsernameExist = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken: cancellationToken);

        if (isUsernameExist != null) return Result<Unit>.Failure(new ConflictException($"Username {request.Username} already exists."));

        var user = new User
        {
            Username = request.Username
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}