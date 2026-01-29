using Dashboard.Constants;
using Dashboard.Contracts.Authentication;
using Dashboard.Contracts.CMS;
using Dashboard.Contracts.General;
using Dashboard.Models;
using Dashboard.Providers;
using Microsoft.JSInterop;
using Shared.DTOs.User;
using Shared.DTOs.User.Customer;
using Shared.DTOs.User.OAuth;
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
        private readonly TokenAuthenticationStateProvider _authStateProvider;
        private readonly ITokenStorageService _tokenStorage;
        private readonly IJSRuntime _jsRuntime;

        public CookieAuthenticationService(
            IApiService apiService,
            TokenAuthenticationStateProvider authStateProvider,
            ITokenStorageService tokenStorage,
            IJSRuntime jsRuntime)
        {
            _apiService = apiService;
            _authStateProvider = authStateProvider;
            _tokenStorage = tokenStorage;
            _jsRuntime = jsRuntime;
        }

        public async Task<ResponseModel<SignInResult>> Login(LoginRequestModel model)
        {
            // Convert LoginRequestModel to the DTO expected by the API
            var loginDto = new IdentifierLoginDto
            {
                Identifier = model.Identifier,
                Password = model.Password
            };

            var response = await _apiService.PostAsync<IdentifierLoginDto, SignInResult>(
                ApiEndpoints.Auth.Login,
                loginDto);

            if (response.Success && response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.Token))
                {
                    try
                    {
                        await _tokenStorage.SetTokensAsync(
                            response.Data.Token,
                            response.Data.RefreshToken ?? string.Empty,
                            response.Data.Email ?? model.Identifier);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CookieAuthService] Failed to store tokens: {ex.Message}");
                    }
                }

                await _authStateProvider.MarkUserAsAuthenticated();

                return new ResponseModel<SignInResult>
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                };
            }

            return new ResponseModel<SignInResult>
            {
                Success = false,
                Message = response.Message ?? "Invalid credentials"
            };
        }

        public async Task<ResponseModel<CustomerRegistrationResponseDto>> RegisterCustomer(CustomerRegistrationDto model)
        {
            var response = await _apiService.PostAsync<CustomerRegistrationDto, CustomerRegistrationResponseDto>(
                "api/v1/auth/register-customer",
                model);

            if (response.Success && response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.Token))
                {
                    try
                    {
                        await _tokenStorage.SetTokensAsync(
                            response.Data.Token,
                            response.Data.RefreshToken ?? string.Empty,
                            response.Data.Email ?? string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CookieAuthService] Failed to store tokens: {ex.Message}");
                    }
                }

                return new ResponseModel<CustomerRegistrationResponseDto>
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                };
            }

            return new ResponseModel<CustomerRegistrationResponseDto>
            {
                Success = false,
                Message = response.Message ?? "Registration failed"
            };
        }

        public async Task<ResponseModel<SignInResult>> GoogleOAuthSignIn(GoogleOAuthTokenDto tokenDto)
        {
            var response = await _apiService.PostAsync<GoogleOAuthTokenDto, SignInResult>(
                "api/v1/auth/google-signin",
                tokenDto);

            if (response.Success && response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.Token))
                {
                    try
                    {
                        await _tokenStorage.SetTokensAsync(
                            response.Data.Token,
                            response.Data.RefreshToken ?? string.Empty,
                            response.Data.Email ?? string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CookieAuthService] Failed to store tokens: {ex.Message}");
                    }
                }

                await _authStateProvider.MarkUserAsAuthenticated();

                return new ResponseModel<SignInResult>
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                };
            }

            return new ResponseModel<SignInResult>
            {
                Success = false,
                Message = response.Message ?? "Google sign-in failed"
            };
        }

        public async Task<ResponseModel<SignInResult>> FacebookOAuthSignIn(FacebookOAuthTokenDto tokenDto)
        {
            var response = await _apiService.PostAsync<FacebookOAuthTokenDto, SignInResult>(
                "api/v1/auth/facebook-signin",
                tokenDto);

            if (response.Success && response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.Token))
                {
                    try
                    {
                        await _tokenStorage.SetTokensAsync(
                            response.Data.Token,
                            response.Data.RefreshToken ?? string.Empty,
                            response.Data.Email ?? string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CookieAuthService] Failed to store tokens: {ex.Message}");
                    }
                }

                await _authStateProvider.MarkUserAsAuthenticated();

                return new ResponseModel<SignInResult>
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                };
            }

            return new ResponseModel<SignInResult>
            {
                Success = false,
                Message = response.Message ?? "Facebook sign-in failed"
            };
        }

        public async Task Logout()
        {
            try
            {
                await _tokenStorage.ClearTokensAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CookieAuthService] Failed to clear tokens: {ex.Message}");
            }

            await _authStateProvider.MarkUserAsLoggedOut();
        }

        /// <summary>
        /// Checks if the user is authenticated by calling the auth state provider.
        /// /// </summary>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var result = await _authStateProvider.IsAuthenticatedAsync();
            return result;
        }

        public async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                // Call the API to refresh the token
                // The refresh token cookie will be sent automatically
                var response = await _apiService.PostAsync<object, object>(
                    ApiEndpoints.Token.AccessToken,
                    new { } // Empty body, cookies are sent automatically
                );

                if (response.Success)
                {
                    // New token is set in cookie by the API
                    await _authStateProvider.MarkUserAsAuthenticated();
                    return true;
                }

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
            var request = new ForgetPasswordRequestDto { Identifier = email };
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
