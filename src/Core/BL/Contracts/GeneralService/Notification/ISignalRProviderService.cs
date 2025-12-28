using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Contracts.GeneralService.Notification;

public interface ISignalRProviderService
{
    Task<ResponseModel<object>> SendNotificationToUser(SignalRNotificationRequest signalRNotificationRequest);
    Task<ResponseModel<object>> SendNotificationToAllUsers(SignalRNotificationRequest signalRNotificationRequest);
}