using Api.Controllers.Base;
using BL.Contracts.GeneralService.CMS;
using Domains.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;
using System.Security.Claims;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthenticationController : BaseController
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService,
            UserManager<ApplicationUser> userManager,
            Serilog.ILogger logger) : base(logger)
        {
            _userAuthenticationService = userAuthenticationService;
            _userManager = userManager;
        }

        /// <summary>
        /// Initiates a password reset by sending a reset token to the user's contact.
        /// </summary>
        [HttpPost("forget-password")]
        [AllowAnonymous]
        public async Task<IActionResult> InitiatePasswordReset([FromBody] ForgetPasswordRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Ok(new ResponseModel<object>
                {
                    Success = false,
                    Message = ValidationResources.EmailRequired
                });
            }

            var result = await _userAuthenticationService.SendResetCodeAsync(request.Email);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = ValidationResources.PasswordResetCodeSent
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? ValidationResources.PasswordResetCodeFailed
            });
        }

        /// <summary>
        /// Completes the password reset by updating the user's password.
        /// </summary>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> CompletePasswordReset([FromBody] ResetPasswordWithCodeDto resetDto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInput,
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _userAuthenticationService.ResetPasswordWithCodeAsync(resetDto.Email, resetDto.VerificationCode, resetDto.NewPassword);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = ValidationResources.PasswordResetSuccess
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = ValidationResources.PasswordResetFailed
            });
        }

        /// <summary>
        /// Deletes the authenticated user's account.
        /// </summary>
        [HttpGet("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var result = await _userAuthenticationService.DeleteAccountAsync(UserId);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = ValidationResources.DeleteSuccess
                });
            }

            return BadRequest(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? NotifiAndAlertsResources.DeleteFailed
            });
        }

        /// <summary>
        /// Gets the current user's information including roles (for authentication state).
        /// Used by Blazor WASM to build authentication claims.
        /// </summary>
        [HttpGet("userinfo")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseModel<UserInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel<UserInfoDto>
                {
                    Success = false,
                    Message = "User not authenticated"
                });
            }

            // Get user from database
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new ResponseModel<UserInfoDto>
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Create response DTO
            var userInfo = new UserInfoDto
            {
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                ProfileImagePath = user.ProfileImagePath,
                Roles = roles.ToList()
            };

            return Ok(new ResponseModel<UserInfoDto>
            {
                Success = true,
                Data = userInfo
            });
        }
    }
}
