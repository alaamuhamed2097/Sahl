namespace Dashboard.Providers
{
    /// <summary>
    /// Cookie-based authentication state provider for Blazor WebAssembly.
    /// Works with HTTP-only cookies set by the API server.
    /// Checks authentication by calling a lightweight API endpoint.
    /// </summary>
    //public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly IJSRuntime _jsRuntime;
    //    private readonly string _baseUrl;
    //    private AuthenticationState? _cachedAuthState;
    //    private DateTime _lastAuthCheck = DateTime.MinValue;
    //    private readonly TimeSpan _cacheTimeout = TimeSpan.FromSeconds(3);
    //    private readonly SemaphoreSlim _authSemaphore = new SemaphoreSlim(1, 1);

    //    public CookieAuthenticationStateProvider(
    //        HttpClient httpClient,
    //        IJSRuntime jsRuntime,
    //        IOptions<ApiSettings> apiSettings)
    //    {
    //        _httpClient = httpClient;
    //        _jsRuntime = jsRuntime;
    //        _baseUrl = apiSettings.Value.BaseUrl;
    //    }

    //    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        // Check cache first (outside semaphore for quick return)
    //        if (_cachedAuthState != null && DateTime.Now - _lastAuthCheck < _cacheTimeout)
    //        {
    //            return _cachedAuthState;
    //        }

    //        // Use semaphore to ensure only one auth check at a time
    //        await _authSemaphore.WaitAsync();

    //        try
    //        {
    //            // Double-check cache after acquiring semaphore
    //            if (_cachedAuthState != null && DateTime.Now - _lastAuthCheck < _cacheTimeout)
    //            {
    //                return _cachedAuthState;
    //            }

    //            // ? PROPER FIX: Check authentication by calling API with cookies
    //            // The browser automatically sends HTTP-only cookies with this request
    //            var userInfoUrl = $"{_baseUrl}{ApiEndpoints.UserAuthentication.UserInfo}";

    //            var userInfoResponse = await FetchWithCredentialsAsync(userInfoUrl, "GET");

    //            if (userInfoResponse.Ok)
    //            {
    //                var userInfoJson = userInfoResponse.Body;

    //                if (string.IsNullOrWhiteSpace(userInfoJson))
    //                {
    //                    var anonymousState = CreateAnonymousState();
    //                    UpdateCache(anonymousState);
    //                    return anonymousState;
    //                }

    //                var userInfo = JsonSerializer.Deserialize<UserInfoResponse>(userInfoJson, new JsonSerializerOptions
    //                {
    //                    PropertyNameCaseInsensitive = true
    //                });

    //                if (userInfo?.Success == true && userInfo.Data != null)
    //                {
    //                    var claims = CreateClaimsFromUserInfo(userInfo.Data);
    //                    var identity = new ClaimsIdentity(claims, "cookie");
    //                    var user = new ClaimsPrincipal(identity);
    //                    var authState = new AuthenticationState(user);

    //                    UpdateCache(authState);

    //                    // ? Sync localStorage flag for backward compatibility
    //                    try
    //                    {
    //                        await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", true);
    //                    }
    //                    catch { /* Ignore JS interop errors */ }

    //                    return authState;
    //                }
    //            }

    //            try
    //            {
    //                await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", false);
    //            }
    //            catch { /* Ignore errors clearing auth state */ }

    //            var unauthState = CreateAnonymousState();
    //            UpdateCache(unauthState);
    //            return unauthState;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[AuthStateProvider] Error checking authentication: {ex.Message}");
    //            var unauthState = CreateAnonymousState();
    //            UpdateCache(unauthState);
    //            return unauthState;
    //        }
    //        finally
    //        {
    //            _authSemaphore.Release();
    //        }
    //    }

    //    public async Task MarkUserAsAuthenticated()
    //    {
    //        // Clear cache immediately to force fresh auth state check
    //        _cachedAuthState = null;
    //        _lastAuthCheck = DateTime.MinValue;

    //        // Set localStorage flag for backward compatibility
    //        try
    //        {
    //            await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", true);
    //        }
    //        catch { /* Ignore JS interop errors */ }

    //        await NotifyAuthenticationStateChanged();
    //    }

    //    public async Task MarkUserAsLoggedOut()
    //    {
    //        try
    //        {
    //            var logoutUrl = $"{_baseUrl}api/Auth/logout";
    //            await FetchWithCredentialsAsync(logoutUrl, "POST");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[AuthStateProvider] Error during logout: {ex.Message}");
    //        }

    //        try
    //        {
    //            await _jsRuntime.InvokeVoidAsync("httpClientHelper.setAuthState", false);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[AuthStateProvider] Error clearing auth state: {ex.Message}");
    //        }

    //        var anonymousState = CreateAnonymousState();
    //        UpdateCache(anonymousState);
    //        NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
    //    }

    //    private AuthenticationState CreateAnonymousState()
    //    {
    //        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    //    }

    //    private void UpdateCache(AuthenticationState authState)
    //    {
    //        _cachedAuthState = authState;
    //        _lastAuthCheck = DateTime.Now;
    //    }

    //    private async Task NotifyAuthenticationStateChanged()
    //    {
    //        var authState = await GetAuthenticationStateAsync();
    //        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    //    }

    //    private List<Claim> CreateClaimsFromUserInfo(UserInfoData userInfo)
    //    {
    //        var claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId ?? string.Empty),
    //            new Claim(ClaimTypes.Name, userInfo.UserName ?? string.Empty),
    //            new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty),
    //            new Claim("fullName", userInfo.FullName ?? string.Empty),
    //            new Claim("userImage", userInfo.ProfileImagePath ?? string.Empty)
    //        };

    //        // Add roles as claims
    //        if (userInfo.Roles != null && userInfo.Roles.Any())
    //        {
    //            foreach (var role in userInfo.Roles)
    //            {
    //                claims.Add(new Claim(ClaimTypes.Role, role));
    //            }
    //        }

    //        return claims;
    //    }

    //    public async Task<bool> IsAuthenticatedAsync()
    //    {
    //        try
    //        {
    //            var authState = await GetAuthenticationStateAsync();
    //            var isAuth = authState.User.Identity?.IsAuthenticated ?? false;
    //            return isAuth;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[AuthStateProvider] Error in IsAuthenticatedAsync: {ex.Message}");
    //            return false;
    //        }
    //    }

    //    private async Task<FetchResponse> FetchWithCredentialsAsync(string url, string method, object? body = null)
    //    {
    //        try
    //        {
    //            var result = await _jsRuntime.InvokeAsync<FetchResponse>(
    //                "httpClientHelper.fetchWithCredentials",
    //                url,
    //                method,
    //                body,
    //                null
    //            );

    //            return result;
    //        }
    //        catch (JSException jsEx)
    //        {
    //            Console.WriteLine($"[AuthStateProvider] JavaScript error: {jsEx.Message}");
    //            return new FetchResponse
    //            {
    //                Ok = false,
    //                Status = 0,
    //                StatusText = jsEx.Message,
    //                Body = string.Empty
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[AuthStateProvider] Fetch error: {ex.Message}");
    //            return new FetchResponse
    //            {
    //                Ok = false,
    //                Status = 0,
    //                StatusText = ex.Message,
    //                Body = string.Empty
    //            };
    //        }
    //    }

    //    private class FetchResponse
    //    {
    //        public bool Ok { get; set; }
    //        public int Status { get; set; }
    //        public string StatusText { get; set; } = string.Empty;
    //        public string Body { get; set; } = string.Empty;
    //    }
    //}

    public class UserInfoResponse
    {
        public bool Success { get; set; }
        public UserInfoData? Data { get; set; }
    }

    public class UserInfoData
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? ProfileImagePath { get; set; }
        public List<string>? Roles { get; set; }
    }
}
