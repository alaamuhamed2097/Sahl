using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;

namespace Api.Controllers.v1.User
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserActivationController : BaseController
    {
        private readonly IUserActivationService _userActivationService;

        public UserActivationController(IUserActivationService userActivationService)
        {
            _userActivationService = userActivationService;
        }

        /// <summary>
        /// Verifies the activation code for a user.
        /// </summary>
        [HttpPost("verify-code")]
        [Authorize]
        public async Task<IActionResult> VerifyActivationCode([FromBody] VerifyActivationCodeDto request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    //Message = "Invalid input.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _userActivationService.VerifyPhoneNumberActivationCodeAsync(UserId, request.ActivationCode);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = result.Message ?? "Activation code verified successfully."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? UserResources.InvalidOrExpiredCode,
                ErrorCode = result.ErrorCode
            });

        }

        /// <summary>
        /// Resends the activation code to the user's mobile.
        /// </summary>
        [HttpPost("send-code")]
        [Authorize]
        public async Task<IActionResult> sendActivationCode([FromBody] SendActivationCodeDto request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid input.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _userActivationService.SendPhoneNumberActivationCodeAsync(UserId, request.PhoneCode, request.PhoneNumber);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Activation code has been resent successfully."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? UserResources.InvalidOrExpiredCode,
                ErrorCode = result.ErrorCode
            });

        }

        /// <summary>
        /// Sends verification code for changing phone number.
        /// </summary>
        [HttpPost("change-phone-send-code")]
        [Authorize]
        public async Task<IActionResult> SendChangePhoneCode([FromBody] ChangePhoneRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid input.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _userActivationService.SendChangePhoneNumberCodeAsync(UserId, request.PhoneCode, request.PhoneNumber);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = result.Message ?? "Verification code sent successfully."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? "Failed to send verification code.",
                ErrorCode = result.ErrorCode,
                Errors = result.Errors
            });
        }

        /// <summary>
        /// Verifies the code and changes the phone number.
        /// </summary>
        [HttpPost("change-phone-verify-code")]
        [Authorize]
        public async Task<IActionResult> VerifyChangePhoneCode([FromBody] VerifyChangePhoneDto request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid input.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var result = await _userActivationService.VerifyChangePhoneNumberCodeAsync(UserId, request.PhoneCode, request.PhoneNumber, request.Code);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = result.Message ?? "Phone number changed successfully."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? UserResources.InvalidOrExpiredCode,
                ErrorCode = result.ErrorCode,
                Errors = result.Errors
            });
        }
    }
}
