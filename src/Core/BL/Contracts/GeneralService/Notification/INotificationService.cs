using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Contracts.GeneralService.Notification;

public interface INotificationService
{
    Task<ResponseModel<object>> SendBulkNotificationAsync(List<NotificationRequest> requests);
    Task<ResponseModel<object>> SendNotificationAsync(NotificationRequest request);
}