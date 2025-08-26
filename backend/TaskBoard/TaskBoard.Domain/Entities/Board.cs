using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class Board : BaseAuditableEntity
{
    public List<User> Users { get; set; } = [];
    public List<Column> Columns { get; set; } = [];
    public List<TaskActivityLog> TaskActivityLogs { get; set; } = [];
}