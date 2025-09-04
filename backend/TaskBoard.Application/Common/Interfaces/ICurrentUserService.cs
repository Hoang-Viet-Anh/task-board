namespace TaskBoard.Application.Common.Interfaces;

public interface ICurrentUserService
{
    public Guid GetUserId();
    public string GetUsername();
}