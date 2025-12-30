using Bl.Contracts.GeneralService.Notification;
using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Service.Setting;
using Common.Enumerations.Notification;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.ErrorCodes;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;
using System.ComponentModel.DataAnnotations;

namespace BL.GeneralService.UserManagement;

public class UserActivationService : IUserActivationService
{
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly INotificationService _notificationService;
    private readonly ISettingService _settingsService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    private string _whatsAppNumber;

    public UserActivationService(
        IVerificationCodeService verificationCodeService,
        INotificationService notificationService,
        ISettingService settingsService,
        UserManager<ApplicationUser> userManager,
        ILogger logger)
    {
        _verificationCodeService = verificationCodeService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _userManager = userManager;
        _logger = logger;
        _whatsAppNumber = (_settingsService.GetAllAsync().Result).FirstOrDefault()?.WhatsAppNumber ?? string.Empty;
    }

    public async Task<OperationResult> SendPhoneNumberActivationCodeAsync(string userId, string phoneCode, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(phoneCode) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            return new OperationResult
            {
                Success = false,
                Message = "User ID and phone number are required",
                ErrorCode = ErrorCodes.Validation.MissingFields
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new OperationResult
            {
                Success = false,
                Message = "User not found",
                ErrorCode = ErrorCodes.User.NotFound
            };
        }

        if (phoneNumber != user.PhoneNumber && phoneCode != user.PhoneCode)
        {
            return new OperationResult
            {
                Success = false,
                Message = "Phone number does not match user's registered phone number",
                ErrorCode = ErrorCodes.User.PhoneNumberMismatch
            };
        }

        var result = await _verificationCodeService.SendCodeAsync(
            phoneCode + phoneNumber,
            NotificationChannel.Sms,
            NotificationType.PhoneNumberVerification,
            new Dictionary<string, string> { { "WhatsApp", _whatsAppNumber } });

        return new OperationResult
        {
            Success = result.Success,
            Message = result.Success ? "Verification code sent successfully" : "Failed to send verification code",
            Errors = result.Errors,
            ErrorCode = result.Success ? null : ErrorCodes.System.UnexpectedError
        };
    }

    public async Task<OperationResult> VerifyPhoneNumberActivationCodeAsync(string userId, string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User ID and code are required",
                    ErrorCode = ErrorCodes.Validation.MissingFields
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.UserState == UserStateType.Deleted)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User not found",
                    ErrorCode = ErrorCodes.User.NotFound
                };
            }

            if (user.UserState == UserStateType.Inactive)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Account is inactive",
                    ErrorCode = ErrorCodes.User.Inactive
                };
            }

            if (!_verificationCodeService.VerifyCode(user.PhoneCode + user.PhoneNumber, code))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Invalid or expired verification code",
                    ErrorCode = ErrorCodes.Auth.InvalidVerificationCode
                };
            }

            var activationResult = await ActivateUserAsync(user);
            if (!activationResult.Success)
            {
                return activationResult;
            }

            _verificationCodeService.DeleteCode(user.Email);

            return new OperationResult
            {
                Success = true,
                Message = "User activated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error verifying activation code for {UserId}", userId);
            return new OperationResult
            {
                Success = false,
                Message = "An error occurred during verification",
                ErrorCode = ErrorCodes.System.UnexpectedError
            };
        }
    }

    public async Task<OperationResult> SendChangePhoneNumberCodeAsync(string userId, string phoneCode, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(phoneCode) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            return new OperationResult
            {
                Success = false,
                Message = "User ID and phone number are required",
                ErrorCode = ErrorCodes.Validation.MissingFields
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new OperationResult
            {
                Success = false,
                Message = "User not found",
                ErrorCode = ErrorCodes.User.NotFound
            };
        }

        // Check if the new phone number is different from the current one
        if (phoneNumber == user.PhoneNumber && phoneCode == user.PhoneCode)
        {
            return new OperationResult
            {
                Success = false,
                Message = "New phone number must be different from the current one",
                ErrorCode = ErrorCodes.User.PhoneNumberDataInvalid
            };
        }

        // Check availability (basic)
        // Ideally checking against normalized phone is better, but simple check against all users might be heavy.
        // Assuming Identity UpdateAsync will handle conflict if enforced.

        var result = await _verificationCodeService.SendCodeAsync(
            phoneCode + phoneNumber,
            NotificationChannel.Sms,
            NotificationType.PhoneNumberVerification,
            new Dictionary<string, string> { { "WhatsApp", _whatsAppNumber } });

        return new OperationResult
        {
            Success = result.Success,
            Message = result.Success ? "Verification code sent to new phone number" : "Failed to send verification code",
            Errors = result.Errors,
            ErrorCode = result.Success ? null : ErrorCodes.System.UnexpectedError
        };
    }

    public async Task<OperationResult> VerifyChangePhoneNumberCodeAsync(string userId, string phoneCode, string phoneNumber, string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(phoneCode) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(code))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User ID, phone number, and code are required",
                    ErrorCode = ErrorCodes.Validation.MissingFields
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.UserState == UserStateType.Deleted)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User not found",
                    ErrorCode = ErrorCodes.User.NotFound
                };
            }

            if (!_verificationCodeService.VerifyCode(phoneCode + phoneNumber, code))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Invalid or expired verification code",
                    ErrorCode = ErrorCodes.Auth.InvalidVerificationCode
                };
            }

            // Update Phone Number
            user.PhoneCode = phoneCode;
            user.PhoneNumber = phoneNumber;
            user.PhoneNumberConfirmed = true;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to update phone number",
                    Errors = result.Errors.Select(e => e.Description).ToList(),
                    ErrorCode = ErrorCodes.System.UnexpectedError
                };
            }

            _verificationCodeService.DeleteCode(phoneCode + phoneNumber);

            return new OperationResult
            {
                Success = true,
                Message = "Phone number changed successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error changing phone number for {UserId}", userId);
            return new OperationResult
            {
                Success = false,
                Message = "An error occurred during phone number change",
                ErrorCode = ErrorCodes.System.UnexpectedError
            };
        }
    }

    public async Task<OperationResult> SendEmailActivationCodeAsync(string userId, string email)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email))
        {
            return new OperationResult
            {
                Success = false,
                Message = "User ID and email are required",
                ErrorCode = ErrorCodes.Validation.MissingFields
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new OperationResult
            {
                Success = false,
                Message = "User not found",
                ErrorCode = ErrorCodes.User.NotFound
            };
        }

        if (email != user.Email)
        {
            if (!new EmailAddressAttribute().IsValid(email))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Invalid email format",
                    ErrorCode = ErrorCodes.Validation.InvalidEmail
                };
            }

            return new OperationResult
            {
                Success = false,
                Message = "Email does not match user's registered email",
                ErrorCode = ErrorCodes.User.EmailMismatch
            };
        }

        var result = await _verificationCodeService.SendCodeAsync(
            email,
            NotificationChannel.Email,
            NotificationType.EmailVerification,
            new Dictionary<string, string> { { "WhatsApp", _whatsAppNumber } });

        return new OperationResult
        {
            Success = result.Success,
            Message = result.Success ? "Verification code sent successfully" : "Failed to send verification code",
            Errors = result.Errors,
            ErrorCode = result.Success ? null : ErrorCodes.System.UnexpectedError
        };
    }

    public async Task<OperationResult> VerifyEmailActivationCodeAsync(string userId, string email, string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User ID, new email, and code are required",
                    ErrorCode = ErrorCodes.Validation.MissingFields
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "User not found",
                    ErrorCode = ErrorCodes.User.NotFound
                };
            }

            if (!_verificationCodeService.VerifyCode(email.ToLower(), code))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Invalid or expired verification code",
                    ErrorCode = ErrorCodes.Auth.InvalidVerificationCode
                };
            }

            if (!new EmailAddressAttribute().IsValid(email))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Invalid email format",
                    ErrorCode = ErrorCodes.Validation.InvalidEmail
                };
            }

            var oldEmail = user.Email;
            var changeEmailResult = await ChangeUserEmailAsync(user, email);
            if (!changeEmailResult.Success)
            {
                return changeEmailResult;
            }

            _verificationCodeService.DeleteCode(oldEmail);

            // Send notification to old email
            var oldEmailNotification = new NotificationRequest
            {
                Recipient = oldEmail,
                Channel = NotificationChannel.Email,
                Type = NotificationType.OldEmailChanged,
                Parameters = CreateNotificationParameters(user, oldEmail, email)
            };
            await _notificationService.SendNotificationAsync(oldEmailNotification);

            // Send activation code to new email no needs to send verification code because here he is already enter one and already changed the email
            //var newEmailCodeSent = await _verificationCodeService.SendCodeAsync(
            //    newEmail,
            //    NotificationChannel.Email,
            //    NotificationType.NewEmailActivation,
            //    CreateNotificationParameters(user, newEmail, newEmail));

            //if (!newEmailCodeSent.Success)
            //{
            //    return new OperationResult
            //    {
            //        Success = false,
            //        Message = "Failed to complete email change process",
            //        Errors = newEmailCodeSent.Errors,
            //        ErrorCode = ErrorCodes.System.UnexpectedError
            //    };
            //}

            return new OperationResult
            {
                Success = true,
                Message = "Email changed successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error changing email for {UserId}", userId);
            return new OperationResult
            {
                Success = false,
                Message = "An error occurred during email change",
                ErrorCode = ErrorCodes.System.UnexpectedError
            };
        }
    }

    #region Helpers Function
    private Dictionary<string, string> CreateNotificationParameters(ApplicationUser user, string oldEmail, string newEmail)
    {
        return new Dictionary<string, string>
            {
                { "FullName", $"{user.FirstName} {user.LastName}" },
                { "OldEmail", oldEmail },
                { "NewEmail", newEmail },
                { "Time", DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm tt") },
                { "WhatsApp", _whatsAppNumber }
            };
    }

    private async Task<OperationResult> ActivateUserAsync(ApplicationUser user)
    {
        user.PhoneNumberConfirmed = true;
        user.UserState = UserStateType.Active;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return new OperationResult
            {
                Success = false,
                Message = "User activation failed",
                Errors = updateResult.Errors.Select(e => e.Description).ToList(),
                ErrorCode = ErrorCodes.System.UnexpectedError
            };
        }

        return new OperationResult { Success = true };
    }

    private async Task<OperationResult> ChangeUserEmailAsync(ApplicationUser user, string newEmail)
    {
        newEmail = newEmail.ToLower();
        var existingUser = await _userManager.FindByEmailAsync(newEmail);

        if (existingUser != null && existingUser.Id != user.Id)
        {
            return new OperationResult
            {
                Success = false,
                Message = "This email is already registered to another account",
                ErrorCode = ErrorCodes.User.EmailExists
            };
        }

        user.Email = newEmail;
        user.UserName = newEmail;
        user.EmailConfirmed = true;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return new OperationResult
            {
                Success = false,
                Message = "Email change failed",
                Errors = updateResult.Errors.Select(e => e.Description).ToList(),
                ErrorCode = ErrorCodes.System.UnexpectedError
            };
        }

        return new OperationResult { Success = true };
    }
    #endregion
}
