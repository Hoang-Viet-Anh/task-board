using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Common.Interfaces;

public interface IJwtProviderService
{
    public string Create(User user);
}