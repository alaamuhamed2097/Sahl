using BL.Contracts.GeneralService.CMS;
using Common.Enumerations.User;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Resources;
using Shared.DTOs.User.OAuth;
using Shared.GeneralModels.ResultModels;
using System.Text.Json;
using SignInResult = Shared.GeneralModels.ResultModels.SignInResult;

namespace BL.GeneralService.CMS;

public class OAuthService : IOAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserAuthenticationService _authenticationService;
    private readonly IUserTokenService _tokenService;
    private readonly Serilog.ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OAuthService(
        UserManager<ApplicationUser> userManager,
        IUserAuthenticationService authenticationService,
        IUserTokenService tokenService,
        Serilog.ILogger logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<ServiceResult<SignInResult>> GoogleSignInAsync(string idToken, string clientType)
    {
        try
        {
            // Verify Google ID token
            var googleSettings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    _configuration["GoogleOAuth:ClientId"] ?? ""
                }
            };

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, googleSettings);
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Invalid Google ID token");
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "Invalid Google token"
                };
            }

            // Extract user info from payload
            var googleUser = new GoogleOAuthResponseDto
            {
                Sub = payload.Subject,
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture,
                EmailVerified = payload.EmailVerified,
                GivenName = payload.GivenName,
                FamilyName = payload.FamilyName
            };

            // Find or create user
            var user = await _userManager.FindByEmailAsync(googleUser.Email);

            if (user == null)
            {
                // Create new user
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = googleUser.Email,
                    UserName = GenerateUniqueUsername(googleUser.Email),
                    FirstName = googleUser.GivenName ?? googleUser.Name ?? "Google",
                    LastName = googleUser.FamilyName ?? "User",
                    ProfileImagePath = googleUser.Picture ?? "uploads/Images/ProfileImages/Customers/default.png",
                    EmailConfirmed = googleUser.EmailVerified,
                    UserState = UserStateType.Active,
                    CreatedBy = Guid.Empty,
                    CreatedDateUtc = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    _logger.Error("Failed to create user from Google OAuth: {Errors}",
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));

                    return new ServiceResult<SignInResult>
                    {
                        Success = false,
                        Message = "Failed to create user account",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Add to Customer role
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            else if (user.UserState == UserStateType.Deleted)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "This account has been deleted"
                };
            }
            else if (user.UserState == UserStateType.Suspended)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "Your account has been suspended"
                };
            }

            // Update last login and security stamp
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id, userRoles);
            if (!tokenResult.Success)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "Failed to generate token"
                };
            }

            // Create refresh token
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, clientType);

            return new ServiceResult<SignInResult>
            {
                Success = true,
                Message = "Google sign-in successful",
                Data = new SignInResult
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    //Email = user.Email,
                    ProfileImagePath = user.ProfileImagePath,
                    Token = tokenResult.Token,
                    RefreshToken = refreshToken,
                    Role = userRoles.FirstOrDefault() ?? "Customer"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Google sign-in error");
            return new ServiceResult<SignInResult>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrong
            };
        }
    }

    public async Task<ServiceResult<SignInResult>> FacebookSignInAsync(string accessToken, string clientType)
    {
        try
        {
            // Verify Facebook token and get user info
            var facebookUser = await GetFacebookUserAsync(accessToken);
            if (facebookUser == null)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "Invalid Facebook token"
                };
            }

            // Find or create user
            var user = await _userManager.FindByEmailAsync(facebookUser.Email);

            if (user == null)
            {
                // Create new user
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = facebookUser.Email,
                    UserName = GenerateUniqueUsername(facebookUser.Email),
                    FirstName = facebookUser.FirstName ?? facebookUser.Name ?? "Facebook",
                    LastName = facebookUser.LastName ?? "User",
                    ProfileImagePath = facebookUser.Picture ?? "uploads/Images/ProfileImages/Customers/default.png",
                    EmailConfirmed = true,
                    UserState = UserStateType.Active,
                    CreatedBy = Guid.Empty,
                    CreatedDateUtc = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    _logger.Error("Failed to create user from Facebook OAuth: {Errors}",
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));

                    return new ServiceResult<SignInResult>
                    {
                        Success = false,
                        Message = "Failed to create user account",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Add to Customer role
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            else if (user.UserState == UserStateType.Deleted)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "This account has been deleted"
                };
            }
            else if (user.UserState == UserStateType.Suspended)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "Your account has been suspended"
                };
            }

            // Update last login and security stamp
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id, userRoles);
            if (!tokenResult.Success)
            {
                return new ServiceResult<SignInResult>
                {
                    Success = false,
                    Message = "Failed to generate token"
                };
            }

            // Create refresh token
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, clientType);

            return new ServiceResult<SignInResult>
            {
                Success = true,
                Message = "Facebook sign-in successful",
                Data = new SignInResult
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    //Email = user.Email,
                    ProfileImagePath = user.ProfileImagePath,
                    Token = tokenResult.Token,
                    RefreshToken = refreshToken,
                    Role = userRoles.FirstOrDefault() ?? "Customer"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Facebook sign-in error");
            return new ServiceResult<SignInResult>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrong
            };
        }
    }

    private async Task<FacebookOAuthResponseDto?> GetFacebookUserAsync(string accessToken)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"https://graph.facebook.com/me?fields=id,email,name,picture.type(large),first_name,last_name&access_token={accessToken}";

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.Error("Facebook API error: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            using (JsonDocument doc = JsonDocument.Parse(content))
            {
                var root = doc.RootElement;
                var pictureUrl = root.GetProperty("picture").GetProperty("data").GetProperty("url").GetString();

                return new FacebookOAuthResponseDto
                {
                    Id = root.GetProperty("id").GetString() ?? "",
                    Email = root.GetProperty("email").GetString() ?? "",
                    Name = root.GetProperty("name").GetString() ?? "",
                    Picture = pictureUrl,
                    FirstName = root.TryGetProperty("first_name", out var fn) ? fn.GetString() : null,
                    LastName = root.TryGetProperty("last_name", out var ln) ? ln.GetString() : null
                };
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error fetching Facebook user data");
            return null;
        }
    }

    private string GenerateUniqueUsername(string email)
    {
        var baseUsername = email.Split('@')[0];
        var username = baseUsername;
        int counter = 1;

        // Check if username is already taken
        while (_userManager.Users.Any(u => u.UserName == username))
        {
            username = $"{baseUsername}{counter++}";
        }

        return username;
    }
}
