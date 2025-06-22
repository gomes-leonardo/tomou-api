using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.Services.Auth;
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public long GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedException();

        return long.Parse(userId);
    }

    public bool IsCaregiver()
    {
        var isCaregiver = _httpContextAccessor.HttpContext?.User.FindFirst("isCaregiver")?.Value;

        if (string.IsNullOrWhiteSpace(isCaregiver))
            throw new UnauthorizedException();

        return bool.Parse(isCaregiver);
    }
}
