using Asp.Versioning;
using BL.Contracts.GeneralService.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.DTOs.User.OAuth;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;
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
        private readonly IOAuthService _oauthService;
        private readonly IUserTokenService _tokenService;
        private readonly Serilog.ILogger _logger;

        public AuthController(
            IUserAuthenticationService authenticationService,
            IOAuthService oauthService,
            IUserTokenService userTokenService,
            Serilog.ILogger logger)
        {
            _authenticationService = authenticationService;
            _tokenService = userTokenService;
            _oauthService = oauthService;
            _logger = logger;
        }

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
                .EmailOrPhoneNumberSignInAsync(loginDto.Identifier, loginDto.Password, clientType);

            if (result.Success)
            {
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

        [HttpPost("login-customer")]
        [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CustomerLogin([FromBody] PhoneLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Ok(new ResponseModel<CustomerSignInResult>
                {
                    Success = false,
                    Errors = errors,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            var result = await _authenticationService
                .CustomerSignInAsync(loginDto, clientType);

            if (result.Success)
            {
                var role = result.Role ?? string.Empty;
                if (!string.Equals(role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(new ResponseModel<CustomerSignInResult>
                    {
                        Success = false,
                        Message = "This endpoint is for customers only. Please use the appropriate login for vendors or administrators."
                    });
                }

                var response = new ResponseModel<CustomerSignInResult>
                {
                    Data = result
                };

                response.SetSuccessMessage(NotifiAndAlertsResources.LoginSuccessful);
                return Ok(response);
            }

            return Ok(new ResponseModel<CustomerSignInResult>
            {
                Success = false,
                Message = result.Message ?? ValidationResources.InvalidLoginAttempt
            });
        }

        [HttpPost("login-vendor")]
        [ProducesResponseType(typeof(ResponseModel<SignInResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VendorLogin([FromBody] EmailLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Ok(new ResponseModel<VendorSignInResult>
                {
                    Success = false,
                    Errors = errors,
                    Message = ValidationResources.PleaseFixValidationErrors
                });
            }

            var clientType = Request.Headers.ContainsKey("X-Platform")
                ? Request.Headers["X-Platform"].ToString().ToLower()
                : "website";

            var result = await _authenticationService
                .VendorSignInAsync(loginDto, clientType);

            if (result.Success)
            {
                var response = new ResponseModel<VendorSignInResult>
                {
                    Data = result
                };

                response.SetSuccessMessage(NotifiAndAlertsResources.LoginSuccessful);
                return Ok(response);
            }

            return Ok(new ResponseModel<VendorSignInResult>
            {
                Success = false,
                Message = result.Message ?? ValidationResources.InvalidLoginAttempt
            });
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

        // أضف هذا الـ endpoint في AuthController.cs

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ResponseModel<RefreshTokenResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.RefreshToken))
            {
                return Unauthorized(new ResponseModel<RefreshTokenResponseDto>
                {
                    Success = false,
                    Message = "Email and refresh token are required"
                });
            }

            try
            {
                var clientType = Request.Headers.ContainsKey("X-Platform")
                    ? Request.Headers["X-Platform"].ToString().ToLower()
                    : "website";

                var validationResult = await _tokenService.ValidateRefreshTokenAsync(
                    new RefreshTokenRequestDto
                    {
                        Email = request.Email,
                        RefreshToken = request.RefreshToken
                    },
                    clientType
                );

                if (!validationResult.Success)
                {
                    return Unauthorized(new ResponseModel<RefreshTokenResponseDto>
                    {
                        Success = false,
                        Message = validationResult.Message ?? "Invalid or expired refresh token"
                    });
                }

                var newAccessToken = await _tokenService.GenerateJwtTokenAsync(
                    validationResult.UserId,
                    validationResult.UserRoles
                );

                if (!newAccessToken.Success)
                {
                    return Unauthorized(new ResponseModel<RefreshTokenResponseDto>
                    {
                        Success = false,
                        Message = "Failed to generate new access token"
                    });
                }

                var regenerateResult = await _tokenService.RegenerateRefreshTokenAsync(
                    new RefreshTokenRequestDto
                    {
                        Email = request.Email,
                        RefreshToken = request.RefreshToken
                    },
                    clientType
                );

                var response = new ResponseModel<RefreshTokenResponseDto>
                {
                    Data = new RefreshTokenResponseDto
                    {
                        AccessToken = newAccessToken.Token,
                        RefreshToken = regenerateResult.Success
                            ? regenerateResult.RefreshToken
                            : request.RefreshToken
                    }
                };

                response.SetSuccessMessage("Token refreshed successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Token refresh error");
                return Unauthorized(new ResponseModel<RefreshTokenResponseDto>
                {
                    Success = false,
                    Message = "An error occurred while refreshing token"
                });
            }
        }
    }
}
