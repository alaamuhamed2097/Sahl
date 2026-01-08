using Asp.Versioning;
using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User.Customer;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;

namespace Api.Controllers.v1.User
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _registrationService;
        private readonly Serilog.ILogger _logger;

        public UserRegistrationController(
            IUserRegistrationService registrationService,
            Serilog.ILogger logger)
        {
            _registrationService = registrationService;
            _logger = logger;
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
        /// Registers a new vendor account.
        /// </summary>
        [HttpPost("register-vendor")]
        [ProducesResponseType(typeof(ResponseModel<VendorRegistrationResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterVendor([FromBody] RegisterVendorRequestDto registerDto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Ok(new ResponseModel<VendorRegistrationResponseDto>
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
                var result = await _registrationService.RegisterVendorAsync(registerDto, clientType);

                if (result.Success && result.Data != null)
                {
                    // Set HTTP-only cookies
                    SetAuthCookies(result.Data.Token, result.Data.RefreshToken);

                    var response = new ResponseModel<VendorRegistrationResponseDto>
                    {
                        Data = result.Data
                    };
                    response.SetSuccessMessage(NotifiAndAlertsResources.RegistrationSuccessful);
                    return Ok(response);
                }

                return Ok(new ResponseModel<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = result.Message ?? NotifiAndAlertsResources.RegistrationFailed,
                    Errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Vendor registration error");
                return Ok(new ResponseModel<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrong
                });
            }
        }

        #region Helpers Methods

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

        #endregion
    }
}
