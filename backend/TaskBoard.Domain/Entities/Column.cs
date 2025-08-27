using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class Column : BaseAuditableEntity
{
    public required Guid BoardId { get; set; }
    public required Board Board { get; set; }
    public required string Title { get; set; }
    public List<Task> Tasks { get; set; } = [];
}