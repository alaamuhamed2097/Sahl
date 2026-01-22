namespace Dashboard.Contracts.Authentication;

public interface ITokenStorageService
{
    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();
    Task<string?> GetUserEmailAsync();
    Task SetTokensAsync(string accessToken, string refreshToken, string userEmail);
    Task ClearTokensAsync();
}