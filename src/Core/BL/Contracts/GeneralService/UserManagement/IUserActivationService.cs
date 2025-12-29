using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.UserManagement;

public interface IUserActivationService
{
    Task<OperationResult> SendActivationCodeAsync(string userId, string identifire);
    Task<OperationResult> VerifyActivationCodeAsync(string userId, string code);
    Task<OperationResult> VerifyNewEmailActivationCodeAsync(string userId, string newEmail, string code);
    Task<OperationResult> VerifyNewPhoneNumberActivationCodeAsync(string userId, string newPhoneNumber, string code);
}
