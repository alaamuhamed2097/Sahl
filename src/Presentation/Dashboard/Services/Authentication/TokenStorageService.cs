using Dashboard.Contracts.Authentication;
using Microsoft.JSInterop;

namespace Dashboard.Services.Authentication;

public class TokenStorageService : ITokenStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public TokenStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> GetUserEmailAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userEmail");
        }
        catch
        {
            return null;
        }
    }

    public async Task SetTokensAsync(string accessToken, string refreshToken, string userEmail)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", accessToken);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userEmail", userEmail);
        await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", true);
    }

    public async Task ClearTokensAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userEmail");
        await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", false);
    }
}