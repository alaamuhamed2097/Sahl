using BL.OperationResults;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface INotificationService
    {
        Task<OperationResult> SendNotificationAsync(NotificationRequest request);
        Task<BulkOperationResult> SendBulkNotificationAsync(NotificationRequest request, IEnumerable<string> recipients);
    }
}
