using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface ISmsProviderService
    {
        Task<OperationResult> SendAsync(SmsRequest request);
    }
}
