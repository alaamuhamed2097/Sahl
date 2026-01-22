using Dashboard.Configuration;
using Dashboard.Contracts.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Dashboard.Services.Authentication;

public class TokenRefreshService : ITokenRefreshService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ITokenStorageService _tokenStorage;
    private readonly string _baseUrl;

    // Lock to prevent multiple simultaneous refresh attempts
    private static readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);
    private static Task<TokenRefreshResult>? _ongoingRefresh;

    public TokenRefreshService(
        IJSRuntime jsRuntime,
        ITokenStorageService tokenStorage,
        IOptions<ApiSettings> apiSettings)
    {
        _jsRuntime = jsRuntime;
        _tokenStorage = tokenStorage;
        _baseUrl = apiSettings.Value.BaseUrl;
    }

    public async Task<TokenRefreshResult> RefreshTokenAsync()
    {
        // If refresh is already in progress, wait for it
        if (_ongoingRefresh != null)
        {
            return await _ongoingRefresh;
        }

        await _refreshLock.WaitAsync();

        try
        {
            // Double-check after acquiring lock
            if (_ongoingRefresh != null)
            {
                return await _ongoingRefresh;
            }

            // Start refresh and store the task
            _ongoingRefresh = PerformRefreshAsync();
            return await _ongoingRefresh;
        }
        finally
        {
            _ongoingRefresh = null;
            _refreshLock.Release();
        }
    }

    private async Task<TokenRefreshResult> PerformRefreshAsync()
    {
        try
        {
            var refreshToken = await _tokenStorage.GetRefreshTokenAsync();
            var userEmail = await _tokenStorage.GetUserEmailAsync();

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userEmail))
            {
                return new TokenRefreshResult
                {
                    Success = false,
                    RequiresLogin = true,
                    Message = "No refresh token or email available"
                };
            }

            // Call refresh endpoint
            var refreshUrl = $"{_baseUrl}api/Auth/refresh";

            var requestBody = new
            {
                email = userEmail,
                refreshToken = refreshToken
            };

            var response = await _jsRuntime.InvokeAsync<FetchResponse>(
                "httpClientHelper.fetchWithCredentials",
                refreshUrl,
                "POST",
                requestBody,
                null
            );

            if (!response.Ok)
            {
                // Token refresh failed - user needs to login again
                await _tokenStorage.ClearTokensAsync();

                return new TokenRefreshResult
                {
                    Success = false,
                    RequiresLogin = true,
                    Message = "Refresh token expired or invalid"
                };
            }

            var result = JsonSerializer.Deserialize<RefreshTokenResponse>(
                response.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (result?.Success == true && !string.IsNullOrEmpty(result.Data?.AccessToken))
            {
                // Store new tokens
                await _tokenStorage.SetTokensAsync(
                    result.Data.AccessToken,
                    result.Data.RefreshToken ?? refreshToken,
                    userEmail
                );

                return new TokenRefreshResult
                {
                    Success = true,
                    NewAccessToken = result.Data.AccessToken,
                    Message = "Token refreshed successfully"
                };
            }

            return new TokenRefreshResult
            {
                Success = false,
                RequiresLogin = true,
                Message = "Invalid refresh response"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TokenRefresh] Error: {ex.Message}");
            return new TokenRefreshResult
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    private class FetchResponse
    {
        public bool Ok { get; set; }
        public int Status { get; set; }
        public string Body { get; set; } = string.Empty;
    }

    private class RefreshTokenResponse
    {
        public bool Success { get; set; }
        public TokenData? Data { get; set; }
        public string? Message { get; set; }
    }

    private class TokenData
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}

