using BL.Contracts.GeneralService.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;
using SignInResult = Shared.GeneralModels.ResultModels.SignInResult;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAuthenticationService _authenticationService;
    private readonly Serilog.ILogger _logger;

    public AuthController(IUserAuthenticationService authenticationService, Serilog.ILogger logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    /// <summary>
    /// Logs in a user using their email/username and password.
    /// Sets HTTP-only cookies for authentication tokens.
    /// </summary>
    /// <param name="loginDto">The login data transfer object containing the identifier and password.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an IActionResult 
    /// that represents the result of the login operation. 
    /// Returns 200 OK if login is successful with cookies set, 400 Bad Request if validation fails, 
    /// or 500 Internal Server Error if an unexpected error occurs.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] IdentifierLoginDto loginDto)
    {
        try
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Errors = errors,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            // Attempt to sign in the user
            var result = await _authenticationService
                .EmailOrUserNameSignInAsync(loginDto.Identifier, loginDto.Password, clientType);

            if (result.Success)
            {
                // ✅ NEW: Set HTTP-only cookies for tokens
                SetAuthCookies(result.Token, result.RefreshToken);

                // ✅ UPDATED: Return user info WITHOUT tokens in response body
                var response = new ResponseModel<SignInResult>
                {
                    //Data = new SignInResult
                    //{
                    //    Success = true,
                    //    FirstName = result.FirstName,
                    //    LastName = result.LastName,
                    //    Email = result.Email,
                    //    HasReachTargetAccountType = result.HasReachTargetAccountType,
                    //    ProfileImagePath = result.ProfileImagePath,
                    //    Role = result.Role,
                    //    // ✅ Don't send tokens in response body
                    //    Token = result.Token,
                    //    RefreshToken = result.RefreshToken
                    //}
                    Data = result
                };

                response.SetSuccessMessage(NotifiAndAlertsResources.LoginSuccessful);
                return Ok(response);
            }

            // Handle login failure
            return Ok(new ResponseModel<SignInResult>
            {
                Success = false,
                Message = result.Message ?? ValidationResources.InvalidLoginAttempt
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred during email or username login for user: {Identifier}", loginDto.Identifier);
            var response = new ResponseModel<object>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrongAlert,
                Errors = new List<string> { "An unexpected error occurred. Please try again later." } // Avoid exposing ex.Message in production
            };
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }

    /// <summary>
    /// Logs out the current user by clearing authentication cookies.
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        try
        {
            // Clear authentication cookies
            ClearAuthCookies();

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Logged out successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred during logout");
            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = "Logout failed"
            });
        }
    }

    /// <summary>
    /// Sets HTTP-only authentication cookies.
    /// </summary>
    private void SetAuthCookies(string accessToken, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // ✅ Prevent JavaScript access
            Secure = true,   // ✅ Only send over HTTPS
            SameSite = SameSiteMode.None, // ✅ Allow cross-origin requests with credentials
            Expires = DateTimeOffset.UtcNow.AddHours(1), // Access token expiration
            Path = "/"
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7), // Refresh token expiration
            Path = "/"
        };

        // Set cookies
        Response.Cookies.Append("auth_token", accessToken, cookieOptions);
        Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);

        _logger.Information("Authentication cookies set successfully");
    }

    /// <summary>
    /// Clears authentication cookies.
    /// </summary>
    private void ClearAuthCookies()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(-1), // Expire immediately
            Path = "/"
        };

        Response.Cookies.Delete("auth_token", cookieOptions);
        Response.Cookies.Delete("refresh_token", cookieOptions);

        _logger.Information("Authentication cookies cleared successfully");
    }
}