using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class TaskActivityLog : BaseAuditableEntity
{
    public required Guid BoardId { get; set; }
    public required Board Board { get; set; }
    public Guid? TaskId { get; set; }
    public Task? Task { get; set; }
    public required Guid UserId { get; set; }
    public required User User { get; set; }
    public required string Log { get; set; }
}