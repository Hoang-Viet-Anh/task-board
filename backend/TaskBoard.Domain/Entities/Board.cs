using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class Board : BaseAuditableEntity
{
    public required string Title { get; set; }
    public required string InviteCode { get; set; }
    public required Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public List<UserBoard> UserBoards { get; set; } = [];
    public List<Column> Columns { get; set; } = [];
    public List<TaskActivityLog> TaskActivityLogs { get; set; } = [];
}