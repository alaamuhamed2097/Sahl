namespace Dashboard.Contracts.Notification
{
    public interface INotificationStateService
    {
        event Action<bool> OnNotificationRead;

        void TriggerNotificationRead();
    }
}