using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Contracts.GeneralService.Notification
{
    public interface ISmsProviderService
    {
        Task<ResponseModel<object>> SendAsync(SmsRequest request);
    }
}