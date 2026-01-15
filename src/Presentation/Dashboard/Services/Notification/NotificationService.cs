using Dashboard.Contracts.Notification;
using Microsoft.JSInterop;

namespace Dashboard.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IJSRuntime _jsRuntime;

        public NotificationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowSuccessAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("showNotification", "success", message);
        }

        public async Task ShowErrorAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("showNotification", "error", message);
        }
    }
}
