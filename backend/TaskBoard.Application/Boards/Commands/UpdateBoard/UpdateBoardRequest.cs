namespace TaskBoard.Application.Boards.Commands.UpdateBoard;

public record UpdateBoardRequest(Guid BoardId, string BoardTitle);