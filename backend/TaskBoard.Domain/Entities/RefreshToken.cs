using TaskBoard.Domain.Common;

namespace TaskBoard.Domain.Entities;

public class RefreshToken : BaseAuditableEntity
{
    public required string TokenHash { get; set; }
    public required Guid UserId { get; set; }
    public required User User { get; set; }
    public required DateTime ExpiresAt { get; set; }
}