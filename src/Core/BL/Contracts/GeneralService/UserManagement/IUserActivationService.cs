using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IUserActivationService
    {
        Task<bool> SendActivationCodeAsync(string mobile);
        Task<OperationResult> VerifyActivationCodeAsync(string mobile, string code);
        Task<bool> ResendActivationCodeAsync(string mobile);
    }
}
