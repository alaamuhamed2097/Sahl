using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Models;
using Dashboard.Providers;
using Microsoft.JSInterop;
using Shared.DTOs.User;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;

namespace Dashboard.Services.CMS
{
    /// <summary>
    /// Cookie-based authentication service for Blazor WebAssembly.
    /// Works with HTTP-Only cookies instead of LocalStorage.
    /// </summary>
    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly IApiService _apiService;
        private readonly CookieAuthenticationStateProvider _authStateProvider;
        private readonly IJSRuntime _jsRuntime;

        public CookieAuthenticationService(
            IApiService apiService,
            CookieAuthenticationStateProvider authStateProvider,
            IJSRuntime jsRuntime)
        {
            _apiService = apiService;
            _authStateProvider = authStateProvider;
            _jsRuntime = jsRuntime;
        }

        public async Task<ResponseModel<SignInResult>> Login(LoginRequestModel model)
        {
            Console.WriteLine("[CookieAuthService] Starting login process...");

            // Convert LoginRequestModel to the DTO expected by the API
            var loginDto = new IdentifierLoginDto
            {
                Identifier = model.Identifier,
                Password = model.Password
            };

            var response = await _apiService.PostAsync<IdentifierLoginDto, SignInResult>(
                ApiEndpoints.Auth.Login,
                loginDto);

            Console.WriteLine($"[CookieAuthService] Login API response: Success={response.Success}");

            if (response.Success && response.Data != null)
            {
                Console.WriteLine("[CookieAuthService] Login successful, updating auth state...");

                // ? CRITICAL FIX: Store token in localStorage for Bearer authentication
                if (!string.IsNullOrEmpty(response.Data.Token))
                {
                    try
                    {
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.Data.Token);
                        Console.WriteLine("[CookieAuthService] ? Token stored in localStorage");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CookieAuthService] ? Failed to store token: {ex.Message}");
                    }
                }

                // Notify auth state provider
                await _authStateProvider.MarkUserAsAuthenticated();
                Console.WriteLine("[CookieAuthService] ? Authentication state provider notified");

                return new ResponseModel<SignInResult>
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                };
            }

            Console.WriteLine($"[CookieAuthService] Login failed: {response.Message}");
            return new ResponseModel<SignInResult>
            {
                Success = false,
                Message = response.Message ?? "Invalid credentials"
            };
        }

        public async Task Logout()
        {
            Console.WriteLine("[CookieAuthService] Starting logout...");

            // Clear token from localStorage
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
                Console.WriteLine("[CookieAuthService] ? Token removed from localStorage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CookieAuthService] ? Failed to remove token: {ex.Message}");
            }

            // The API will clear the cookies
            await _authStateProvider.MarkUserAsLoggedOut();
            Console.WriteLine("[CookieAuthService] ? Logout complete");
        }

        /// <summary>
        /// Checks if the user is authenticated by calling the auth state provider.
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var result = await _authStateProvider.IsAuthenticatedAsync();
            Console.WriteLine($"[CookieAuthService] IsAuthenticatedAsync result: {result}");
            return result;
        }

        public async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                Console.WriteLine("[CookieAuthService] Attempting token refresh...");

                // Call the API to refresh the token
                // The refresh token cookie will be sent automatically
                var response = await _apiService.PostAsync<object, object>(
                    ApiEndpoints.Token.AccessToken,
                    new { } // Empty body, cookies are sent automatically
                );

                if (response.Success)
                {
                    Console.WriteLine("[CookieAuthService] ? Token refreshed successfully");
                    // New token is set in cookie by the API
                    await _authStateProvider.MarkUserAsAuthenticated();
                    return true;
                }

                Console.WriteLine("[CookieAuthService] ? Token refresh failed, logging out");
                await Logout(); // If refresh failed, log out user
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CookieAuthService] ? Token refresh error: {ex.Message}");
                await Logout();
                return false;
            }
        }

        public async Task<ResponseModel<object>> SendResetPasswordCode(string email)
        {
            var request = new ForgetPasswordRequestDto { Email = email };
            return await _apiService.PostAsync<ForgetPasswordRequestDto, object>(
                "api/UserAuthentication/forget-password",
                request);
        }

        public async Task<ResponseModel<object>> ResetPasswordWithCode(ResetPasswordWithCodeDto model)
        {
            return await _apiService.PostAsync<ResetPasswordWithCodeDto, object>(
                "api/UserAuthentication/reset-password",
                model);
        }
    }
}
