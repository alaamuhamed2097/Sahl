using Asp.Versioning;
using BL.Contracts.GeneralService.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;
using SignInResult = Shared.GeneralModels.ResultModels.SignInResult;

namespace Api.Controllers.v1.Authentication
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
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
                // ? NEW: Set HTTP-only cookies for tokens
                SetAuthCookies(result.Token, result.RefreshToken);

                // ? UPDATED: Return user info WITHOUT tokens in response body
                var response = new ResponseModel<SignInResult>
                {
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

        /// <summary>
        /// Logs out the current user by clearing authentication cookies.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // Clear authentication cookies
            ClearAuthCookies();

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Logged out successfully"
            });
        }

        /// <summary>
        /// Sets HTTP-only authentication cookies.
        /// </summary>
        private void SetAuthCookies(string accessToken, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                Path = "/"
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                Path = "/"
            };

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
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                Path = "/"
            };

            Response.Cookies.Delete("auth_token", cookieOptions);
            Response.Cookies.Delete("refresh_token", cookieOptions);

            _logger.Information("Authentication cookies cleared successfully");
        }
    }
}
