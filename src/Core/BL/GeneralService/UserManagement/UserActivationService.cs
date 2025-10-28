using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.UserManagement;
using Common.Enumerations.User;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Resources;
using Shared.GeneralModels.ResultModels;

namespace BL.GeneralService.UserManagement
{
    public class UserActivationService : IUserActivationService
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserActivationService(
            IVerificationCodeService verificationCodeService,
            UserManager<ApplicationUser> userManager)
        {
            _verificationCodeService = verificationCodeService;
            _userManager = userManager;
        }

        /// <summary>
        /// Sends a new activation code to the user's mobile.
        /// </summary>
        public async Task<bool> SendActivationCodeAsync(string mobile)
        {
            return await _verificationCodeService.SendCodeAsync(mobile);
        }

        /// <summary>
        /// Verifies the activation code and activates the user.
        /// </summary>
        public async Task<OperationResult> VerifyActivationCodeAsync(string mobile, string code)
        {
            // Verify the code
            var isCodeValid = _verificationCodeService.VerifyCode(mobile, code);

            if (!isCodeValid)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = UserResources.InvalidOrExpiredCode
                };
            }

            // Find the user by mobile number
            var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == mobile);

            if (user == null || user.UserState == UserStateType.Deleted)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = UserResources.UserNotFound
                };
            }

            // Activate the user
            user.PhoneNumberConfirmed = true; // Assuming there’s an `IsActive` property
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = UserResources.UserActivationFailed,
                    Errors = updateResult.Errors.Select(e => e.Description).ToList()
                };
            }

            // Remove the used code from cache
            _verificationCodeService.DeleteCode(mobile);

            return new OperationResult
            {
                Success = true,
                Message = UserResources.UserActivatedSuccessfully
            };
        }

        /// <summary>
        /// Resends the activation code to the user's mobile if the cooldown period has elapsed.
        /// </summary>
        public async Task<bool> ResendActivationCodeAsync(string mobile)
        {
            return await _verificationCodeService.SendCodeAsync(mobile);
        }
    }
}
