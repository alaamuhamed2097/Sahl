using Api.Controllers.Base;
using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivationController : BaseController
    {
        private readonly IUserActivationService _userActivationService;

        public UserActivationController(IUserActivationService userActivationService,
            Serilog.ILogger logger) : base(logger)
        {
            _userActivationService = userActivationService;
        }

        /// <summary>
        /// Verifies the activation code for a user.
        /// </summary>
        [HttpPost("verify-code")]
        [AllowAnonymous]
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

            var result = await _userActivationService.VerifyActivationCodeAsync(request.Mobile, request.ActivationCode);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = result.Message ?? ValidationResources.ActivationCodeVerified
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? UserResources.VerificationCodeError
            });
        }

        /// <summary>
        /// Resends the activation code to the user's mobile.
        /// </summary>
        [HttpPost("resend-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendActivationCode([FromBody] ResendActivationCodeDto request)
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

            var success = await _userActivationService.ResendActivationCodeAsync(request.Mobile);

            if (success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = UserResources.ActivationCodeResent
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = UserResources.VerificationCodeError
            });
        }
    }
}
