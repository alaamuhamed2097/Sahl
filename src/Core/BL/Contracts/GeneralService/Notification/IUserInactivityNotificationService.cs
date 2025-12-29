using Shared.GeneralModels.ResultModels;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface IUserInactivityNotificationService
    {
        Task<NotificationResult> SendInactivityNotifications(bool validateTime = true);
    }
}