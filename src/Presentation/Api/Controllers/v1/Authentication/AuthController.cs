using Asp.Versioning;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.DTOs.User.Customer;
using Shared.DTOs.User.OAuth;
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
        private readonly IUserRegistrationService _registrationService;
        private readonly IOAuthService _oauthService;
        private readonly Serilog.ILogger _logger;

        public AuthController(
            IUserAuthenticationService authenticationService,
            IUserRegistrationService registrationService,
            IOAuthService oauthService,
            Serilog.ILogger logger)
        {
            _authenticationService = authenticationService;
            _registrationService = registrationService;
            _oauthService = oauthService;
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
        /// Customer-specific login. Accepts email/username/phone and password but only allows users in the "Customer" role.
        /// Sets HTTP-only cookies for successful authentication.
        /// </summary>
        [HttpPost("login-customer")]
        [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CustomerLogin([FromBody] IdentifierLoginDto loginDto)
        {
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

            // Customer login: accept only email or phone as identifier
            var identifier = loginDto.Identifier?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = "Email or phone number is required."
                });
            }

            var isEmail = identifier.Contains("@") && identifier.Contains(".");
            var normalizedPhone = string.Empty;
            var isPhone = false;

            if (!isEmail)
            {
                normalizedPhone = PhoneNormalizationHelper.NormalizePhone(identifier);
                isPhone = !string.IsNullOrWhiteSpace(normalizedPhone);
            }

            if (!isEmail && !isPhone)
            {
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = "Customer login requires an email address or phone number as identifier."
                });
            }

            var authIdentifier = isEmail ? identifier : normalizedPhone;

            var result = await _authenticationService
                .EmailOrUserNameSignInAsync(authIdentifier, loginDto.Password, clientType);

            if (result.Success)
            {
                var role = result.Role ?? string.Empty;
                if (!string.Equals(role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(new ResponseModel<SignInResult>
                    {
                        Success = false,
                        Message = "This endpoint is for customers only. Please use the appropriate login for vendors or administrators."
                    });
                }

                SetAuthCookies(result.Token, result.RefreshToken);

                var response = new ResponseModel<SignInResult>
                {
                    Data = result
                };

                response.SetSuccessMessage(NotifiAndAlertsResources.LoginSuccessful);
                return Ok(response);
            }

            return Ok(new ResponseModel<SignInResult>
            {
                Success = false,
                Message = result.Message ?? ValidationResources.InvalidLoginAttempt
            });
        }

        /// <summary>
        /// Registers a new customer account.
        /// </summary>
        [HttpPost("register-customer")]
        [ProducesResponseType(typeof(ResponseModel<CustomerRegistrationResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationDto registerDto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Ok(new ResponseModel<CustomerRegistrationResponseDto>
                {
                    Success = false,
                    Errors = errors,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            try
            {
                var result = await _registrationService.RegisterCustomerAsync(registerDto, clientType);

                if (result.Success && result.Data != null)
                {
                    // Set HTTP-only cookies
                    SetAuthCookies(result.Data.Token, result.Data.RefreshToken);

                    var response = new ResponseModel<CustomerRegistrationResponseDto>
                    {
                        Data = result.Data
                    };
                    response.SetSuccessMessage(NotifiAndAlertsResources.RegistrationSuccessful);
                    return Ok(response);
                }

                return Ok(new ResponseModel<CustomerRegistrationResponseDto>
                {
                    Success = false,
                    Message = result.Message ?? NotifiAndAlertsResources.RegistrationFailed,
                    Errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Customer registration error");
                return Ok(new ResponseModel<CustomerRegistrationResponseDto>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrong
                });
            }
        }

        /// <summary>
        /// Sign in or register user via Google OAuth.
        /// </summary>
        [HttpPost("google-signin")]
        [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleOAuthTokenDto tokenDto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            try
            {
                var result = await _oauthService.GoogleSignInAsync(tokenDto.IdToken, clientType);

                if (result.Success && result.Data != null)
                {
                    SetAuthCookies(result.Data.Token, result.Data.RefreshToken);

                    var response = new ResponseModel<SignInResult>
                    {
                        Data = result.Data
                    };
                    response.SetSuccessMessage("Google sign-in successful");
                    return Ok(response);
                }

                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = result.Message ?? "Google sign-in failed"
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Google sign-in error");
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrong
                });
            }
        }

        /// <summary>
        /// Customer-only Google sign-in. Ensures the authenticated/created user has the Customer role.
        /// Sets HTTP-only cookies on success.
        /// </summary>
        [HttpPost("google-customer-signin")]
        [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GoogleCustomerSignIn([FromBody] GoogleOAuthTokenDto tokenDto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            try
            {
                var result = await _oauthService.GoogleSignInAsync(tokenDto.IdToken, clientType);

                if (result.Success && result.Data != null)
                {
                    var role = result.Data.Role ?? string.Empty;
                    if (!string.Equals(role, "customer", StringComparison.OrdinalIgnoreCase))
                    {
                        return Ok(new ResponseModel<SignInResult>
                        {
                            Success = false,
                            Message = "This endpoint is for customers only."
                        });
                    }

                    SetAuthCookies(result.Data.Token, result.Data.RefreshToken);

                    var response = new ResponseModel<SignInResult>
                    {
                        Data = result.Data
                    };
                    response.SetSuccessMessage("Google sign-in successful");
                    return Ok(response);
                }

                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = result.Message ?? "Google sign-in failed"
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Google sign-in error");
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrong
                });
            }
        }

        /// <summary>
        /// Sign in or register user via Facebook OAuth.
        /// </summary>
        [HttpPost("facebook-signin")]
        [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FacebookSignIn([FromBody] FacebookOAuthTokenDto tokenDto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            try
            {
                var result = await _oauthService.FacebookSignInAsync(tokenDto.AccessToken, clientType);

                if (result.Success && result.Data != null)
                {
                    SetAuthCookies(result.Data.Token, result.Data.RefreshToken);

                    var response = new ResponseModel<SignInResult>
                    {
                        Data = result.Data
                    };
                    response.SetSuccessMessage("Facebook sign-in successful");
                    return Ok(response);
                }

                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = result.Message ?? "Facebook sign-in failed"
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Facebook sign-in error");
                return Ok(new ResponseModel<SignInResult>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrong
                });
            }
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
