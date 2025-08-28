using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Common.Interfaces;

public interface IJwtProviderService
{
    public Task<TokenResponse> Create(User user, CancellationToken cancellationToken);
}