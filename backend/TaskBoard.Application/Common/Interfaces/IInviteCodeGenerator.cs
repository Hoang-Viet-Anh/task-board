namespace TaskBoard.Application.Common.Interfaces;

public interface IInviteCodeGenerator
{
    public Task<string> GenerateUniqueInviteCode(CancellationToken cancellationToken);
}