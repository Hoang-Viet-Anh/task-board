using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class Board : BaseAuditableEntity
{
    public List<UserBoard> UserBoards { get; set; } = [];
    public List<Column> Columns { get; set; } = [];
    public List<TaskActivityLog> TaskActivityLogs { get; set; } = [];
}