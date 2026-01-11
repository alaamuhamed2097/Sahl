using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.Service.Vendor;
using BL.Utils;
using Common.Enumerations.Notification;
using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;
using System.Security.Claims;

namespace BL.GeneralService.CMS;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserTokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly IVendorService _vendorService;

    public UserAuthenticationService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserTokenService tokenService,
        IEmailService emailService,
        IVerificationCodeService verificationCodeService,
        IVendorService vendorService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _verificationCodeService = verificationCodeService;
        _vendorService = vendorService;
    }


    public async Task<string> LoginUserAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Identifier)
               ?? await _userManager.FindByNameAsync(loginDto.Identifier);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var roles = await _userManager.GetRolesAsync(user);
            return (await _tokenService.GenerateJwtTokenAsync(user.Id.ToString(), roles)).Token;
        }

        throw new UnauthorizedAccessException("Invalid login credentials.");
    }

    public async Task<Shared.GeneralModels.ResultModels.SignInResult> EmailOrPhoneNumberSignInAsync(string identifier, string password, string clientType, SignInMethod method = SignInMethod.Email)
    {
        try
        {
            ApplicationUser? user = null;

            if (method == SignInMethod.Email)
            {
                // Try to find by email
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else if (method == SignInMethod.PhoneNumber)
            {
                // If still not found, try by phone number
                if (user == null && !string.IsNullOrWhiteSpace(identifier))
                {
                    var normalizedPhone = PhoneNormalizationHelper.NormalizePhone(identifier);
                    if (!string.IsNullOrWhiteSpace(normalizedPhone))
                    {
                        user = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedPhone != null &&
                                                      u.NormalizedPhone.Contains(normalizedPhone));
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid sign-in method.");
            }

            if (user == null)
            {
                return new Shared.GeneralModels.ResultModels.SignInResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new Shared.GeneralModels.ResultModels.SignInResult
                {
                    Success = false,
                    Message = "User is locked out."
                };
            }

            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordCheckResult)
            {
                return new Shared.GeneralModels.ResultModels.SignInResult
                {
                    Success = false,
                    Message = "Invalid password."
                };
            }

            if (user.UserState == UserStateType.Suspended)
            {
                return new Shared.GeneralModels.ResultModels.SignInResult
                {
                    Success = false,
                    Message = "Your account has been suspended by the administration due to a violation of the platform's policies. For more details, please contact technical support."
                };
            }

            // Reset failed access count on successful login
            await _userManager.ResetAccessFailedCountAsync(user);

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Generate new security stamp to invalidate other sessions
            await _userManager.UpdateSecurityStampAsync(user);

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id.ToString(), userRoles);
            if (!tokenResult.Success)
            {
                return new Shared.GeneralModels.ResultModels.SignInResult
                {
                    Success = false,
                    Message = "Failed to generate token."
                };
            }

            // Invalidate all previous refresh tokens (single session)
            await InvalidateAllRefreshTokens(user.Id.ToString());

            // create a refresh token
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, clientType);

            return new Shared.GeneralModels.ResultModels.SignInResult
            {
                Success = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImagePath = user.ProfileImagePath ?? "uploads/Images/ProfileImages/Vendor/default.png",
                Token = tokenResult.Token,
                RefreshToken = refreshToken,
                Role = userRoles[0]
            };
        }
        catch (Exception)
        {
            // Log the exception (ex)
            return new Shared.GeneralModels.ResultModels.SignInResult
            {
                Success = false,
                Message = ValidationResources.InvalidLoginAttempt
            };
        }
    }

    public async Task<CustomerSignInResult> CustomerSignInAsync(PhoneLoginDto request, string clientType)
    {
        try
        {
            ApplicationUser? user = user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneCode == request.PhoneCode &&
                                              u.PhoneNumber == request.PhoneNumber);

            if (user == null)
            {
                return new Shared.GeneralModels.ResultModels.CustomerSignInResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(nameof(UserRole.Customer)))
            {
                return new Shared.GeneralModels.ResultModels.CustomerSignInResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new Shared.GeneralModels.ResultModels.CustomerSignInResult
                {
                    Success = false,
                    Message = "User is locked out."
                };
            }

            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordCheckResult)
            {
                return new Shared.GeneralModels.ResultModels.CustomerSignInResult
                {
                    Success = false,
                    Message = "Invalid password."
                };
            }

            if (user.UserState == UserStateType.Suspended)
            {
                return new Shared.GeneralModels.ResultModels.CustomerSignInResult
                {
                    Success = false,
                    Message = "Your account has been suspended by the administration due to a violation of the platform's policies. For more details, please contact technical support."
                };
            }

            // Reset failed access count on successful login
            await _userManager.ResetAccessFailedCountAsync(user);

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Generate new security stamp to invalidate other sessions
            await _userManager.UpdateSecurityStampAsync(user);

            // Generate JWT token
            var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id.ToString(), userRoles);
            if (!tokenResult.Success)
            {
                return new Shared.GeneralModels.ResultModels.CustomerSignInResult
                {
                    Success = false,
                    Message = "Failed to generate token."
                };
            }

            // Invalidate all previous refresh tokens (single session)
            await InvalidateAllRefreshTokens(user.Id.ToString());

            // create a refresh token
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, clientType);

            return new Shared.GeneralModels.ResultModels.CustomerSignInResult
            {
                Success = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneCode = user.PhoneCode,
                PhoneNumber = user.PhoneNumber,
                ProfileImagePath = user.ProfileImagePath ?? "uploads/Images/ProfileImages/Vendor/default.png",
                Token = tokenResult.Token,
                RefreshToken = refreshToken,
                Role = userRoles[0],
                IsActive = user.PhoneNumberConfirmed
            };
        }
        catch (Exception)
        {
            // Log the exception (ex)
            return new Shared.GeneralModels.ResultModels.CustomerSignInResult
            {
                Success = false,
                Message = ValidationResources.InvalidLoginAttempt
            };
        }
    }

    public async Task<VendorSignInResult> VendorSignInAsync(EmailLoginDto request, string clientType)
    {
        try
        {
            ApplicationUser? user = user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains(nameof(UserRole.Vendor)))
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "User is locked out."
                };
            }

            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordCheckResult)
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "Invalid password."
                };
            }

            if (user.UserState == UserStateType.Suspended)
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "Your account has been suspended by the administration due to a violation of the platform's policies. For more details, please contact technical support."
                };
            }

            var vendor = await _vendorService.GetByUserIdAsync(user.Id);
            if (vendor == null)
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "Vendor profile not found."
                };
            }

            if (vendor.Status == VendorStatus.Rejected)
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "you are rejected."
                };
            }

            // Reset failed access count on successful login
            await _userManager.ResetAccessFailedCountAsync(user);

            // Update last login date
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Generate new security stamp to invalidate other sessions
            await _userManager.UpdateSecurityStampAsync(user);

            // Generate JWT token
            var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id.ToString(), userRoles);
            if (!tokenResult.Success)
            {
                return new Shared.GeneralModels.ResultModels.VendorSignInResult
                {
                    Success = false,
                    Message = "Failed to generate token."
                };
            }

            // Invalidate all previous refresh tokens (single session)
            await InvalidateAllRefreshTokens(user.Id.ToString());

            // create a refresh token
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, clientType);

            return new Shared.GeneralModels.ResultModels.VendorSignInResult
            {
                Success = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImagePath = user.ProfileImagePath ?? "uploads/Images/ProfileImages/Vendor/default.png",
                Token = tokenResult.Token,
                RefreshToken = refreshToken,
                Role = userRoles[0],
                IsActive = user.EmailConfirmed,
                IsVerified = vendor.Status == VendorStatus.Approved
            };
        }
        catch (Exception)
        {
            // Log the exception (ex)
            return new Shared.GeneralModels.ResultModels.VendorSignInResult
            {
                Success = false,
                Message = ValidationResources.InvalidLoginAttempt
            };
        }
    }


    public async Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);

        return result;
    }

    public async Task<OperationResult> ResetPasswordAsync(PasswordResetDto resetDto)
    {
        var response = new OperationResult();

        // Find the user by their email number
        var user = await _userManager.FindByEmailAsync(resetDto.Email);
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
            return response;
        }

        // Generate a password reset token for the user
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // Here you would send this token to the user (via SMS or email) in a real application.

        // Reset the password using the token (this should be done after the user has verified the token)
        var result = await _userManager.ResetPasswordAsync(user, token, resetDto.NewPassword);
        if (result.Succeeded)
        {
            response.Success = true;
            response.Message = "Password reset successfully.";
        }
        else
        {
            response.Success = false;
            response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return response;
    }

    public async Task<OperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var response = new OperationResult();

        // Await the asynchronous user retrieval
        var user = await _userManager.FindByIdAsync(userId);

        // If user is not found, return an error in the response
        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
            return response;
        }

        // Attempt to change the password
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        // Return success or failure in the response based on the result
        if (result.Succeeded)
        {
            response.Success = true;
            response.Message = "Password changed successfully.";
        }
        else
        {
            response.Success = false;
            response.Message = string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return response;
    }

    public async Task<OperationResult> SendResetCodeAsync(string userId, string identifier)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.UserState == UserStateType.Deleted)
        {
            return new OperationResult { Success = false, Message = UserResources.UserNotFound };
        }

        string normalizedIdentifier = identifier;
        bool isEmail = identifier.Contains("@");
        OperationResult codeSaved;

        if (isEmail)
        {
            if (!string.Equals(user.Email, identifier, StringComparison.OrdinalIgnoreCase))
            {
                return new OperationResult { Success = false, Message = "The provided email does not match the user's records." };
            }

            if (!user.EmailConfirmed)
                return new OperationResult { Success = false, Message = "Email is not verified." };

            codeSaved = await _verificationCodeService.SendCodeAsync(normalizedIdentifier, NotificationChannel.Email, NotificationType.ForgotPasswordByEmail);
        }
        else
        {
            normalizedIdentifier = PhoneNormalizationHelper.NormalizePhone(identifier);
            // Check if user's phone matches
            // Note: Since users stores NormalizedPhone, we should compare that
            if (user.NormalizedPhone == null || !user.NormalizedPhone.Contains(normalizedIdentifier))
            {
                return new OperationResult { Success = false, Message = "The provided phone number does not match the user's records." };
            }

            if (!user.PhoneNumberConfirmed)
                return new OperationResult { Success = false, Message = "Phone number is not verified." };

            codeSaved = await _verificationCodeService.SendCodeAsync(normalizedIdentifier, NotificationChannel.Sms, NotificationType.ForgotPasswordByPhone);
        }

        // Save the code temporarily (e.g., in cache) with expiration time
        if (!codeSaved.Success)
        {
            return new OperationResult { Success = false, Message = UserResources.VerificationCodeError };
        }

        return new OperationResult { Success = true, Message = $"Verification code sent to your {(isEmail ? "email" : "phone")}." };
    }

    public async Task<OperationResult> ResetPasswordWithCodeAsync(string userId, string identifier, string code, string newPassword)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return new OperationResult { Success = false, Message = "Identifier is required." };
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            return new OperationResult { Success = false, Message = "Verification code is required." };
        }

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
        {
            return new OperationResult { Success = false, Message = "Password must be at least 6 characters long." };
        }

        string normalizedIdentifier = identifier;
        bool isEmail = identifier.Contains("@");
        if (!isEmail)
        {
            normalizedIdentifier = PhoneNormalizationHelper.NormalizePhone(identifier);
        }

        // Verify the code
        var isCodeValid = _verificationCodeService.VerifyCode(normalizedIdentifier, code);
        if (!isCodeValid)
        {
            return new OperationResult { Success = false, Message = "Invalid or expired code." };
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null || user.UserState == UserStateType.Deleted)
        {
            return new OperationResult { Success = false, Message = UserResources.UserNotFound };
        }

        if (isEmail)
        {
            if (!string.Equals(user.Email, identifier, StringComparison.OrdinalIgnoreCase))
            {
                return new OperationResult { Success = false, Message = "The provided email does not match the user's records." };
            }

            if (!user.EmailConfirmed)
                return new OperationResult { Success = false, Message = "Email is not verified." };
        }
        else
        {
            if (user.NormalizedPhone == null || !user.NormalizedPhone.Contains(normalizedIdentifier))
            {
                return new OperationResult { Success = false, Message = "The provided phone number does not match the user's records." };
            }

            if (!user.PhoneNumberConfirmed)
                return new OperationResult { Success = false, Message = "Phone number is not verified." };
        }

        // Generate password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Reset password
        var resetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!resetResult.Succeeded)
        {
            return new OperationResult { Success = false, Message = "Password reset failed.", Errors = resetResult.Errors.Select(e => e.Description).ToList() };
        }

        // Delete the code after successful reset
        _verificationCodeService.DeleteCode(normalizedIdentifier);

        return new OperationResult { Success = true, Message = "Password reset successful." };
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<OperationResult> DeleteAccountAsync(string userId)
    {
        var response = new OperationResult();

        // Find the user by ID
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null || user.UserState == UserStateType.Deleted)
            return new OperationResult { Success = false, Message = UserResources.UserNotFound };

        // Perform a soft delete by setting the state
        user.UserState = UserStateType.Deleted;
        user.UpdatedDateUtc = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return new OperationResult { Success = true };

        // Collect and log errors
        var errors = result.Errors.Select(e => e.Description).ToList();

        return new OperationResult { Success = false, Message = "Failed to update user status.", Errors = errors };
    }

    public Task<ApplicationUser> GetAuthenticatedUserAsync(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserAuthorizedAsync(ApplicationUser user, string policy)
    {
        throw new NotImplementedException();
    }

    #region Helper Methods

    private async Task InvalidateAllRefreshTokens(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;

        // Get all client types that might have tokens
        var clientTypes = new[] { "Web", "Mobile", "Desktop" }; // Adjust as needed

        foreach (var clientType in clientTypes)
        {
            await _userManager.RemoveAuthenticationTokenAsync(
                user, TokenOptions.DefaultProvider, $"RefreshToken_{clientType}");
            await _userManager.RemoveAuthenticationTokenAsync(
                user, TokenOptions.DefaultProvider, $"RefreshTokenExpiration_{clientType}");
        }
    }

    #endregion
}