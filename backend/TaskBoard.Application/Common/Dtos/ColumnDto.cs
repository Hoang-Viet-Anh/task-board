namespace TaskBoard.Application.Common.Dtos;

public class ColumnDto
{
    public required string Id { get; set; }
    public string? Title { get; set; }
    public List<TaskDto> Tasks { get; set; } = [];
}