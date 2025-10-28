using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using Common.Enumerations.User;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;

namespace BL.GeneralService.CMS
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserTokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IVerificationCodeService _verificationCodeService;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserTokenService tokenService,
            IEmailService emailService,
            IVerificationCodeService verificationCodeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _verificationCodeService = verificationCodeService;
        }

        public async Task<string> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Identifier)
                   ?? await _userManager.FindByNameAsync(loginDto.Identifier);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                return (await _tokenService.GenerateJwtTokenAsync(user.Id, roles)).Token;
            }

            throw new UnauthorizedAccessException("Invalid login credentials.");
        }

        public async Task<Shared.GeneralModels.ResultModels.SignInResult> EmailOrUserNameSignInAsync(string identifier, string password, string clientType)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(identifier)
                        ?? await _userManager.FindByNameAsync(identifier);
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
                        Message = "Your account has been suspended by the administration due to a violation of the platform’s policies. For more details, please contact technical support."
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
                var tokenResult = await _tokenService.GenerateJwtTokenAsync(user.Id, userRoles);
                if (!tokenResult.Success)
                {
                    return new Shared.GeneralModels.ResultModels.SignInResult
                    {
                        Success = false,
                        Message = "Failed to generate token."
                    };
                }

                // Invalidate all previous refresh tokens (single session)
                await InvalidateAllRefreshTokens(user.Id);

                // create a refresh token
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, clientType);

                return new Shared.GeneralModels.ResultModels.SignInResult
                {
                    Success = true,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ProfileImagePath = user.ProfileImagePath ?? "uploads/Images/ProfileImages/Marketers/default.png",
                    Token = tokenResult.Token,
                    RefreshToken = refreshToken,
                    Role = userRoles[0]
                };
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return new Shared.GeneralModels.ResultModels.SignInResult
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
        /// <summary>
        /// Sends a password reset verification code to the user's mobile.
        /// </summary>
        public async Task<OperationResult> SendResetCodeAsync(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.UserState == UserStateType.Deleted)
            {
                return new OperationResult { Success = false, Message = "User with this Email was not found." };
            }

            // Save the code temporarily (e.g., in cache) with expiration time
            var codeSaved = await _verificationCodeService.SendCodeAsync(email);

            if (!codeSaved)
            {
                return new OperationResult { Success = false, Message = UserResources.VerificationCodeError };
            }

            return new OperationResult { Success = true, Message = "Verification code sent to your email." };
        }

        /// <summary>
        /// Resets the user's password using a verification code.
        /// </summary>
        public async Task<OperationResult> ResetPasswordWithCodeAsync(string email, string code, string newPassword)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(email))
            {
                return new OperationResult { Success = false, Message = "Email is required." };
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return new OperationResult { Success = false, Message = "Verification code is required." };
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                return new OperationResult { Success = false, Message = "Password must be at least 8 characters long." };
            }

            // Verify the code
            var isCodeValid = _verificationCodeService.VerifyCode(email, code);

            var attemptsKey = $"{email}_code_attempts";
            int attempts = _verificationCodeService.GetAttempts(email);
            if (attempts >= 5)
            {
                return new OperationResult { Success = false, Message = "Too many attempts" };
            }
            if (!isCodeValid)
            {
                return new OperationResult { Success = false, Message = "Invalid or expired code." };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.UserState == UserStateType.Deleted)
            {
                return new OperationResult { Success = false, Message = "User with this email was not found." };
            }

            // Generate password reset token (if your identity system requires it)
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset password
            var resetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!resetResult.Succeeded)
            {
                return new OperationResult { Success = false, Message = "Password reset failed.", Errors = resetResult.Errors.Select(e => e.Description).ToList() };
            }

            // Optionally, delete the code after successful reset
            _verificationCodeService.DeleteCode(email);

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
    }
}