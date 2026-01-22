using Dashboard.Configuration;
using Dashboard.Constants;
using Dashboard.Contracts.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace Dashboard.Providers
{
    /// <summary>
    /// Token-based authentication state provider for Blazor WebAssembly.
    /// Uses JWT tokens stored in localStorage.
    /// </summary>
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ITokenStorageService _tokenStorage;
        private readonly string _baseUrl;
        private AuthenticationState? _cachedAuthState;
        private DateTime _lastAuthCheck = DateTime.MinValue;
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromSeconds(3);
        private readonly SemaphoreSlim _authSemaphore = new SemaphoreSlim(1, 1);

        public TokenAuthenticationStateProvider(
            IJSRuntime jsRuntime,
            ITokenStorageService tokenStorage,
            IOptions<ApiSettings> apiSettings)
        {
            _jsRuntime = jsRuntime;
            _tokenStorage = tokenStorage;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Check cache first
            if (_cachedAuthState != null && DateTime.Now - _lastAuthCheck < _cacheTimeout)
            {
                return _cachedAuthState;
            }

            await _authSemaphore.WaitAsync();

            try
            {
                // Double-check after acquiring lock
                if (_cachedAuthState != null && DateTime.Now - _lastAuthCheck < _cacheTimeout)
                {
                    return _cachedAuthState;
                }

                // Get token from localStorage
                var accessToken = await _tokenStorage.GetAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("[AuthStateProvider] No access token found");
                    var anonymousState = CreateAnonymousState();
                    UpdateCache(anonymousState);
                    await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", false);
                    return anonymousState;
                }

                // Validate token by calling userinfo endpoint
                var userInfoUrl = $"{_baseUrl}{ApiEndpoints.UserAuthentication.UserInfo}";
                var userInfoResponse = await FetchWithTokenAsync(userInfoUrl, "GET");

                if (userInfoResponse.Ok)
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfoResponse>(
                        userInfoResponse.Body,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (userInfo?.Success == true && userInfo.Data != null)
                    {
                        var claims = CreateClaimsFromUserInfo(userInfo.Data);
                        var identity = new ClaimsIdentity(claims, "Bearer");
                        var user = new ClaimsPrincipal(identity);
                        var authState = new AuthenticationState(user);

                        UpdateCache(authState);
                        await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", true);

                        return authState;
                    }
                }
                else if (userInfoResponse.Status == 401)
                {
                    // Token is invalid or expired
                    Console.WriteLine("[AuthStateProvider] Token invalid (401), clearing tokens");
                    await _tokenStorage.ClearTokensAsync();
                }

                await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", false);
                var unauthState = CreateAnonymousState();
                UpdateCache(unauthState);
                return unauthState;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthStateProvider] Error: {ex.Message}");
                var unauthState = CreateAnonymousState();
                UpdateCache(unauthState);
                return unauthState;
            }
            finally
            {
                _authSemaphore.Release();
            }
        }

        public async Task MarkUserAsAuthenticated()
        {
            // Clear cache to force fresh check
            _cachedAuthState = null;
            _lastAuthCheck = DateTime.MinValue;

            await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", true);
            await NotifyAuthenticationStateChanged();
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _tokenStorage.ClearTokensAsync();

            var anonymousState = CreateAnonymousState();
            UpdateCache(anonymousState);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
        }

        private AuthenticationState CreateAnonymousState()
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private void UpdateCache(AuthenticationState authState)
        {
            _cachedAuthState = authState;
            _lastAuthCheck = DateTime.Now;
        }

        private async Task NotifyAuthenticationStateChanged()
        {
            var authState = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        private List<Claim> CreateClaimsFromUserInfo(UserInfoData userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId ?? string.Empty),
                new Claim(ClaimTypes.Name, userInfo.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty),
                new Claim("fullName", userInfo.FullName ?? string.Empty),
                new Claim("userImage", userInfo.ProfileImagePath ?? string.Empty)
            };

            if (userInfo.Roles != null && userInfo.Roles.Any())
            {
                foreach (var role in userInfo.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            return claims;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var authState = await GetAuthenticationStateAsync();
                return authState.User.Identity?.IsAuthenticated ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthStateProvider] Error in IsAuthenticatedAsync: {ex.Message}");
                return false;
            }
        }

        private async Task<FetchResponse> FetchWithTokenAsync(string url, string method, object? body = null)
        {
            try
            {
                var result = await _jsRuntime.InvokeAsync<FetchResponse>(
                    "httpClientHelper.fetchWithCredentials",
                    url,
                    method,
                    body,
                    null
                );

                return result;
            }
            catch (JSException jsEx)
            {
                Console.WriteLine($"[AuthStateProvider] JavaScript error: {jsEx.Message}");
                return new FetchResponse
                {
                    Ok = false,
                    Status = 0,
                    StatusText = jsEx.Message,
                    Body = string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthStateProvider] Fetch error: {ex.Message}");
                return new FetchResponse
                {
                    Ok = false,
                    Status = 0,
                    StatusText = ex.Message,
                    Body = string.Empty
                };
            }
        }

        private class FetchResponse
        {
            public bool Ok { get; set; }
            public int Status { get; set; }
            public string StatusText { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
        }
    }
}
