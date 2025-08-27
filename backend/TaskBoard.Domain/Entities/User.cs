using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class User : BaseAuditableEntity
{
    public required string Username { get; set; }
    public string? PasswordHash { get; set; }
    public List<UserBoard> UserBoards { get; set; } = [];
    public List<TaskActivityLog> TaskActivityLogs { get; set; } = [];
}