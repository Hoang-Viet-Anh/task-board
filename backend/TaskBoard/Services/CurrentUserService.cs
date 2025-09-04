using System.Security.Claims;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException();
        var guidUserId = Guid.Parse(userId);

        return guidUserId;
    }

    public string GetUsername()
    {
        var username = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? throw new UnauthorizedAccessException();
        return username;
    }
}
