using Blazored.LocalStorage;
using Dashboard.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dashboard.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>("token");

            if (string.IsNullOrWhiteSpace(token) || JwtHelper.IsTokenExpired(token))
            {
                await localStorage.RemoveItemAsync("token");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = JwtHelper.GetClaimsFromToken(token).ToList();

            if (!claims.Any(c => c.Type == ClaimTypes.Name))
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                if (!string.IsNullOrEmpty(jwtToken.Subject))
                    claims.Add(new Claim(ClaimTypes.Name, jwtToken.Subject));
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public async Task LoggedIn()
        {
            var claims = await GetClaims();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoggedOut()
        {
            await localStorage.RemoveItemAsync("token");
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<IEnumerable<Claim>> GetClaims()
        {
            // Retrieve the saved token from local storage
            var savedToken = await localStorage.GetItemAsync<string>("token");

            // Check if the token is null or empty
            if (string.IsNullOrEmpty(savedToken))
            {
                // Return empty claims or throw an exception as needed
                return new List<Claim>();
            }

            // Parse the token
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(savedToken);

            // Ensure that the token has valid claims
            var claims = tokenContent.Claims.ToList();

            // Add the subject claim (but ensure it's not null)
            if (!string.IsNullOrEmpty(tokenContent.Subject))
            {
                claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            }

            return claims;
        }
    }
}
