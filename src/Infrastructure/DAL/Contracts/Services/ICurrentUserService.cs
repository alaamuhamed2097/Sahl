namespace BL.Contracts.GeneralService;

/// <summary>
/// Service for retrieving the current authenticated user's information
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user ID as a string
    /// Returns null if no user is authenticated
    /// </summary>
    string? GetCurrentUserId();

    /// <summary>
    /// Gets the current user ID as a Guid
    /// Returns Guid.Empty if no user is authenticated or ID cannot be parsed
    /// </summary>
    Guid GetCurrentUserIdAsGuid();

    /// <summary>
    /// Checks if a user is currently authenticated
    /// </summary>
    bool IsUserAuthenticated();

    /// <summary>
    /// Gets the current user's role
    /// Returns null if no user is authenticated
    /// </summary>
    string? GetCurrentUserRole();

    /// <summary>
    /// Gets the current user's name/username
    /// Returns null if no user is authenticated
    /// </summary>
    string? GetCurrentUserName();
}
