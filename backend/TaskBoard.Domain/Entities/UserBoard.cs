using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class UserBoard : BaseAuditableEntity
{
    public required Guid UserId { get; set; }
    public required User User { get; set; }
    public required Guid BoardId { get; set; }
    public required Board Board { get; set; }
}