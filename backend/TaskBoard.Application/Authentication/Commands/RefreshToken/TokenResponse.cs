namespace TaskBoard.Application.Authentication.Commands.RefreshToken;

public record TokenResponse(string AccessToken, string RefreshToken);