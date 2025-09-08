namespace TaskBoard.Application.Common.Dtos;

public class TaskActivityLogDto
{
    public required Guid Id { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? UserId { get; set; }
    public string? Log { get; set; }
    public DateTime? CreatedAt { get; set; }
}