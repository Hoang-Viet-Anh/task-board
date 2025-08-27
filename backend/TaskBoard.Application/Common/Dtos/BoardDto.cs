namespace TaskBoard.Application.Common.Dtos;

public class BoardDto
{
    public required string Id { get; set; }
    public string? Title { get; set; }
    public string? InviteCode { get; set; }
    public bool? IsOwner { get; set; }
    public Guid? OwnerId { get; set; }
    public List<ColumnDto> Columns { get; set; } = [];
}