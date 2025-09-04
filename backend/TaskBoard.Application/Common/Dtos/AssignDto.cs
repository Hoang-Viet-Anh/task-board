namespace TaskBoard.Application.Common.Dtos;

public class AssignDto
{
    public required Guid TaskId { get; set; }
    public required Guid AsigneeId { get; set; }
}