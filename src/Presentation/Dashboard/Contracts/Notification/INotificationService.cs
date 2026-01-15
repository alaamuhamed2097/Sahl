namespace Dashboard.Contracts.Notification
{
    public interface INotificationService
    {
        Task ShowSuccessAsync(string message);
        Task ShowErrorAsync(string message);
    }
}
