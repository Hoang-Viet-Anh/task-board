namespace TaskBoard.Application.Common.Dtos;

public class TaskDto
{
    public Guid? Id { get; set; }
    public Guid? ColumnId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Priority { get; set; }
    public List<UserDto> AssignedUsers { get; set; } = [];
    public List<TaskActivityLogDto> TaskActivityLogs { get; set; } = [];
    public DateTime? CreatedAt { get; set; }
}