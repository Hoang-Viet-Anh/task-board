namespace TaskBoard.Application.Common.Dtos;

public class TaskDto
{
    public required string Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Priority { get; set; }
    public List<UserDto> AssignedUsers { get; set; } = [];
}