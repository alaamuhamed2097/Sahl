using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface IEmailProviderService
    {
        Task<OperationResult> SendAsync(EmailRequest request);
        //Task<OperationResult> SendAsync(EmailBCCRequest request);
    }
}
