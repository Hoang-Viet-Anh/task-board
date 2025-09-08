using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class UserTask : BaseAuditableEntity
{
    public required Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public required Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
}