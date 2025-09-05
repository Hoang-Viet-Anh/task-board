namespace TaskBoard.Application.Common.Interfaces;

public interface ISlackService
{
    public Task SendMessage(string message);
}