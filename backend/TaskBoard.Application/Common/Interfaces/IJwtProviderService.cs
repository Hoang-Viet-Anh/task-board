using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Authentication.Commands.RefreshToken;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Common.Interfaces;

public interface IJwtProviderService
{
    public Task<TokenResponse> Create(User user, CancellationToken cancellationToken);
    public void SetJwtCookies(TokenResponse tokens, HttpResponse response);
    public void ClearJwtCookies(HttpResponse response);
}