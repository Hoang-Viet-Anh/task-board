using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class UserBoard : BaseAuditableEntity
{
    public required Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public required Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
}