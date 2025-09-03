namespace TaskBoard.Application.Common.Dtos;

public class TaskActivityLogDto
{
    public required string Id { get; set; }
    public string? TaskId { get; set; }
    public string? UserId { get; set; }
    public string? Log { get; set; }
    public DateTime? CreatedAt { get; set; }
}