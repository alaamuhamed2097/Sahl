using Dashboard.Contracts.Notification;

namespace Dashboard.Services.Notification
{
    public class NotificationStateService : INotificationStateService
    {
        public event Action<bool> OnNotificationRead;

        public void TriggerNotificationRead()
        {
            OnNotificationRead?.Invoke(true);
        }
    }
}