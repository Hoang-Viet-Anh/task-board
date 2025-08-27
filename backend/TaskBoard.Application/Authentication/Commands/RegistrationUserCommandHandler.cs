using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Common.Exceptions;
using TaskBoard.Application.Common.Interfaces;
using TaskBoard.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskBoard.Application.Authentication.Commands;

public record RegistrationUserCommand(string Username, string Password) : IRequest<Task>;

public class RegistrationUserCommandHandler : IRequestHandler<RegistrationUserCommand, Task>
{
    public readonly IApplicationDbContext _context;

    public RegistrationUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Task> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
    {
        var isUsernameExist = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken: cancellationToken);

        if (isUsernameExist != null) throw new ConflictException($"Username {request.Username} already exists.");

        var user = new User
        {
            Username = request.Username
        };
        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        var board = new Board();

        var userBoard = new UserBoard
        {
            UserId = user.Id,
            User = user,
            BoardId = board.Id,
            Board = board
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.Boards.AddAsync(board, cancellationToken);
        await _context.UserBoards.AddAsync(userBoard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Task.CompletedTask;
    }
}