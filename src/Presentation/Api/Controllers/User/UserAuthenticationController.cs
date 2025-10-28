using Api.Controllers.Base;
using BL.Contracts.GeneralService.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthenticationController : BaseController
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService,
            Serilog.ILogger logger) : base(logger)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        /// <summary>
        /// Initiates a password reset by sending a reset token to the user's contact.
        /// </summary>
        [HttpPost("forget-password")]
        [AllowAnonymous]
        public async Task<IActionResult> InitiatePasswordReset([FromBody] ForgetPasswordRequestDto request)
        {
            try
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
                    Message =  result.Message ?? ValidationResources.PasswordResetCodeFailed
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Completes the password reset by updating the user's password.
        /// </summary>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> CompletePasswordReset([FromBody] ResetPasswordWithCodeDto resetDto)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes the authenticated user's account.
        /// </summary>
        [HttpGet("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
