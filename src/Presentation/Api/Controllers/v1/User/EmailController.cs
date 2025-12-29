using Api.Controllers.v1.Base;
using Bl.Contracts.GeneralService.Notification;
using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;
using Shared.ErrorCodes;
using Shared.GeneralModels;

namespace CoursesAcademyAPI.Controllers.User
{
    /// <summary>
    /// Provides endpoints for user profile management, including email updates.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : BaseController
    {
        private readonly IUserProfileService _profileService;
        private readonly IUserActivationService _userActivationService;
        private readonly INotificationService _notificationService;

        public EmailController(
            IUserProfileService profileService,
            IUserActivationService userActivationService,
                        INotificationService notificationService,
            Serilog.ILogger logger)
        {
            _profileService = profileService;
            _notificationService = notificationService;
            _userActivationService = userActivationService;
        }

        /// <summary>
        /// Sends an OTP to the new email to verify before updating.
        /// </summary>
        [HttpPost("change-email")]
        [Authorize]
        public async Task<IActionResult> SendOtpToNewEmail([FromBody] string newEmail)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return Ok(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Invalid input.",
                    Errors = errors,
                    ErrorCode = ErrorCodes.Validation.InvalidFields
                });
            }

            var result = await _profileService.ChangeEmailAsync(UserId, newEmail);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "OTP has been sent to the new email."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? "Failed to send OTP. Please try again.",
                Errors = result.Errors,
                ErrorCode = result.ErrorCode,
            });
        }

        /// <summary>
        /// Verifies the OTP sent to the new email and updates the user’s profile.
        /// </summary>
        [HttpPost("verify-new-email")]
        [Authorize]
        public async Task<IActionResult> VerifyNewEmail([FromBody] VerifyActivationCodeDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return Ok(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Invalid input.",
                    Errors = errors,
                    ErrorCode = ErrorCodes.Validation.InvalidFields
                });
            }

            var result = await _userActivationService.VerifyNewEmailActivationCodeAsync(UserId, dto.Identifire, dto.ActivationCode);

            if (result.Success)
            {
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = result.Message ?? "Email updated successfully."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = false,
                Message = result.Message ?? "Failed to verify OTP or update email.",
                Errors = result.Errors,
                ErrorCode = result.ErrorCode,
            });
        }


        // this api for testing sending emails
        //[HttpPost("send-email")]
        //public async Task<IActionResult> SendEmail([FromBody] string newEmail)
        //{
        //    try
        //    {

        //        var notificationRequest = new NotificationRequest
        //        {
        //            Recipient = newEmail,
        //            Channel = NotificationChannel.Email,
        //            Type = NotificationType.NewEmailActivation,
        //            Parameters = new Dictionary<string, string>
        //            {
        //                { "Time", DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm tt") }
        //            }
        //        };

        //        // Send notification to email
        //        var notificationSentResult = await _notificationService.SendNotificationAsync(notificationRequest);

        //        if (notificationSentResult.Success)
        //        {
        //            return Ok(new ResponseModel<object>
        //            {
        //                Success = true,
        //                Message = "sent to the  email."
        //            });
        //        }

        //        return Ok(new ResponseModel<object>
        //        {
        //            Success = false,
        //            Message = "Failed to send . Please try again.",

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}
    }
}