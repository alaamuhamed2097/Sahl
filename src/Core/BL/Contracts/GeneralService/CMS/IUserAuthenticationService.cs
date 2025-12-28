using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;
using System.Security.Claims;

namespace BL.Contracts.GeneralService.CMS
{
    public interface IUserAuthenticationService
    {
        Task<string> LoginUserAsync(LoginDto loginDto);
        Task<SignInResult> EmailOrPhoneNumberSignInAsync(string identifier, string password, string clientType);
        Task<Microsoft.AspNetCore.Identity.SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
        Task<OperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<OperationResult> ResetPasswordAsync(PasswordResetDto resetDto);
        Task<ApplicationUser> GetAuthenticatedUserAsync(ClaimsPrincipal principal);
        Task<bool> IsUserAuthorizedAsync(ApplicationUser user, string policy);
        Task<OperationResult> SendResetCodeAsync(string email);
        Task<OperationResult> ResetPasswordWithCodeAsync(string email, string code, string newPassword);
        Task<OperationResult> DeleteAccountAsync(string userId);

        Task SignOutAsync();
    }
}
