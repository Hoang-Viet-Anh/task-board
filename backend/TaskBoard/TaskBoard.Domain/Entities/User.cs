using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class User : BaseAuditableEntity
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public List<Board> Boards { get; set; } = [];
    public List<TaskActivityLog> TaskActivityLogs { get; set; } = [];
}