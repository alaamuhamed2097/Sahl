using Blazored.LocalStorage;
using Dashboard.Constants;
using Shared.DTOs.User;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;
using System.IdentityModel.Tokens.Jwt;
using Dashboard.Contracts.General;
using Dashboard.Models;
using Dashboard.Providers;

namespace Dashboard.Services.General
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiService _apiService;
        private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStateProvider _authStateProvider;

        public AuthenticationService(
        IApiService apiService,
        ILocalStorageService localStorage,
        ApiAuthenticationStateProvider authStateProvider)
        {
            _apiService = apiService;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<ResponseModel<SignInResult>> Login(LoginRequestModel model)
        {
            var response = await _apiService.PostAsync<LoginRequestModel, SignInResult>(ApiEndpoints.Auth.EmailLogin, model);
            if (response.Success)
            {
                await _localStorage.SetItemAsync("token", response.Data.Token);
                if (!string.IsNullOrEmpty(response.Data.RefreshToken))
                    await _localStorage.SetItemAsync("refreshToken", response.Data.RefreshToken);

                await _authStateProvider.LoggedIn();
                return response;
            }

            return new ResponseModel<SignInResult> { Success = false, Message = response.Message ?? "Invalid credentials" };
        }

        public async Task Logout()
        {
            var response = await _apiService.GetAsync<string>(ApiEndpoints.Auth.LogOut);
            if (response.Success)
            {
                await _localStorage.RemoveItemAsync("token");
                await _localStorage.RemoveItemAsync("refreshToken");
                await _authStateProvider.LoggedOut();
            }
        }

        /// <summary>
        /// Checks if the user is authenticated.
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("token");

            if (string.IsNullOrWhiteSpace(token))
                return false;

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;

            // Check if token is expired
            // expiry          = 2025-04-19 14:00:00 UTC
            // DateTime.UtcNow = 2025-04-19 15:00:00 UTC
            // if (expiry < DateTime.UtcNow) --> true expiry
            if (expiry < DateTime.UtcNow)
            {
                // Optional: Try refresh here
                var success = await TryRefreshTokenAsync();
                return success;
            }

            return true;
        }


        public async Task<bool> TryRefreshTokenAsync()
        {
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken"); 
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            var response = await _apiService.PostAsync<RefreshTokenDto, GenerateTokenResult>(
                ApiEndpoints.Token.AccessToken,
                new RefreshTokenDto { RefreshToken = refreshToken }
            );

            if (response.Success)
            {
                await _localStorage.SetItemAsync("token", response.Data.Token);
                //await _localStorage.SetItemAsync("refreshToken", response.Data.RefreshToken);

                await _authStateProvider.LoggedIn(); // Notify auth state changed
                return true;
            }

            await Logout(); // If refresh failed, log out user
            return false;
        }

    }
}
