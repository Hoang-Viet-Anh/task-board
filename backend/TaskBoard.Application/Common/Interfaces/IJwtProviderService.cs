using TaskBoard.Application.Common.Dtos;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Common.Interfaces;

public interface IJwtProviderService
{
    public Task<TokenResponse> Create(User user, CancellationToken cancellationToken);
}