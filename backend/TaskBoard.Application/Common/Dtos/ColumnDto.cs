namespace TaskBoard.Application.Common.Dtos;

public class ColumnDto
{
    public string? Id { get; set; }
    public string? BoardId { get; set; }
    public string? Title { get; set; }
    public List<TaskDto> Tasks { get; set; } = [];
    public DateTime? CreatedAt { get; set; }
}