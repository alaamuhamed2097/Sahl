using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;

namespace Dashboard.Contracts.General
{
    public interface IUserAuthenticationService
    {
        Task<string> LoginUserAsync(LoginDto loginDto);
        Task<SignInResult> EmailOrUserNameSignInAsync(string identifier, string password, string clientType);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
        Task<OperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<OperationResult> ResetPasswordAsync(PasswordResetDto resetDto);
        Task<OperationResult> SendResetCodeAsync(string email);
        Task<OperationResult> ResetPasswordWithCodeAsync(string email, string code, string newPassword);
        Task<OperationResult> DeleteAccountAsync(string userId);

        Task SignOutAsync();
    }
}
