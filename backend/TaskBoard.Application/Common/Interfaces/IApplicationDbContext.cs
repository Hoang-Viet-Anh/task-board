using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Board> Boards { get; }
    public DbSet<Column> Columns { get; }
    public DbSet<Domain.Entities.Task> Tasks { get; }
    public DbSet<TaskActivityLog> TaskActivityLogs { get; }
    public DbSet<UserBoard> UserBoards { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}