using Dashboard.Services.Authentication;

namespace Dashboard.Contracts.Authentication;

public interface ITokenRefreshService
{
    Task<TokenRefreshResult> RefreshTokenAsync();
}