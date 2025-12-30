using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.UserManagement;

public interface IUserActivationService
{
    Task<OperationResult> SendPhoneNumberActivationCodeAsync(string userId, string phoneCode, string phoneNumber);
    Task<OperationResult> VerifyPhoneNumberActivationCodeAsync(string userId, string code);

    Task<OperationResult> SendChangePhoneNumberCodeAsync(string userId, string phoneCode, string phoneNumber);
    Task<OperationResult> VerifyChangePhoneNumberCodeAsync(string userId, string phoneCode, string phoneNumber, string code);

    Task<OperationResult> SendEmailActivationCodeAsync(string userId, string email);
    Task<OperationResult> VerifyEmailActivationCodeAsync(string userId, string newEmail, string code);
}
