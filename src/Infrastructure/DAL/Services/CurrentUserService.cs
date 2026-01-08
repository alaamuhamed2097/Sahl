using BL.Contracts.GeneralService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DAL.Services;

/// <summary>
/// Service for retrieving the current authenticated user's information from HttpContext
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Gets the current user ID as a string from the claims principal
    /// </summary>
    public string? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            return null;

        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// Gets the current user ID as a Guid
    /// Returns Guid.Empty if no user is authenticated or ID cannot be parsed
    /// </summary>
    public Guid GetCurrentUserIdAsGuid()
    {
        var userId = GetCurrentUserId();
        return Guid.TryParse(userId, out var guid) ? guid : Guid.Empty;
    }

    /// <summary>
    /// Checks if a user is currently authenticated
    /// </summary>
    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    /// <summary>
    /// Gets the current user's role from claims
    /// </summary>
    public string? GetCurrentUserRole()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            return null;

        return user.FindFirst(ClaimTypes.Role)?.Value;
    }

    /// <summary>
    /// Gets the current user's name/username from claims
    /// </summary>
    public string? GetCurrentUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            return null;

        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
}
