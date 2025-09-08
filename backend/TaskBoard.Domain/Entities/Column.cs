using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class Column : BaseAuditableEntity
{
    public required Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public required string Title { get; set; }
    public required string Order { get; set; }
    public List<Task> Tasks { get; set; } = [];
}