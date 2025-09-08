using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class TaskActivityLog : BaseAuditableEntity
{
    public required Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public Guid? TaskId { get; set; }
    public Task Task { get; set; } = null!;
    public required Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public required string Log { get; set; }
}