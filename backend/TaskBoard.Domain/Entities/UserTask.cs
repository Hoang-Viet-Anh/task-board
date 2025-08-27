using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class UserTask : BaseAuditableEntity
{
    public required Guid UserId { get; set; }
    public required User User { get; set; }
    public required Guid TaskId { get; set; }
    public required Task Task { get; set; }
}