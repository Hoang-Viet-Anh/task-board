namespace TaskBoard.Application.Common.Dtos;

public class ColumnDto
{
    public Guid? Id { get; set; }
    public Guid? BoardId { get; set; }
    public string? Title { get; set; }
    public string? Order { get; set; }
    public List<TaskDto> Tasks { get; set; } = [];
    public DateTime? CreatedAt { get; set; }
}