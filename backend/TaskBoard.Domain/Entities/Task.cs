using TaskBoard.Domain.Common;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Domain.Entities;

public class Task : BaseAuditableEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required DateTime DueDate { get; set; }
    public required TaskPriority Priority { get; set; }
    public required Guid ColumnId { get; set; }
    public Column Column { get; set; } = null!;
    public List<UserTask> UserTasks { get; set; } = [];
    public List<TaskActivityLog> TaskActivityLogs { get; set; } = [];
}