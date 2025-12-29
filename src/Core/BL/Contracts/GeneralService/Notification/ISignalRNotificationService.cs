using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface ISignalRNotificationService
    {
        Task<OperationResult> SendNotificationToUser(SignalRNotificationRequest request);
        Task<OperationResult> SendNotificationToUsers(SignalRNotificationRequest request);
        Task<OperationResult> SendNotificationToAllUsers(SignalRNotificationRequest request);
    }
}
