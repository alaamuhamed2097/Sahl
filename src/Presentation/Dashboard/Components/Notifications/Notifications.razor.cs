using Dashboard.Contracts.Notification;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.GeneralModels.Parameters.Notification;

namespace Dashboard.Components.Notifications
{
    public partial class Notifications
    {
        private string ComponentId = Guid.NewGuid().ToString("N")[..8];
        private bool isScrollbarInitialized = false;
        private int UnReadCount { get; set; }
        private List<UserNotificationRequest> UserNotifications { get; set; } = new List<UserNotificationRequest>();
        private List<UserNotificationRequest> MarkNotifications { get; set; } = new List<UserNotificationRequest>();
        private bool _isLoading = false;

        [Parameter] public bool NotificationsChanged { get; set; }
        [Inject] private IUserNotificationService UserNotificationService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserNotificationsAsync();
        }
        protected override async Task OnParametersSetAsync()
        {
            if (NotificationsChanged)
            {
                await LoadUserNotificationsAsync();
                StateHasChanged();
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || !isScrollbarInitialized)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("initializePerfectScrollbar", $"notification-body-{ComponentId}");
                    isScrollbarInitialized = true;
                }
                catch (Exception)
                {
                    // Ignore JS interop errors
                }
            }
        }
        private async Task HandleNotificationRead(bool value)
        {
            if (value)
            {
                await LoadUserNotificationsAsync();
                StateHasChanged();
            }
        }

        private async Task MarkNotificationsAsReadAsync(IEnumerable<UserNotificationRequest> markNotifications)
        {
            if (MarkNotifications == null || MarkNotifications.Count() == 0)
                return;

            try
            {
                var response = await UserNotificationService.MarkAsReadAsync(MarkNotifications);
                if (response?.Success == true)
                {
                    await LoadUserNotificationsAsync();
                    StateHasChanged();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
                }
            }
            catch (Exception)
            {
                // Silently handle errors - notifications are not critical
            }
            finally
            {
                MarkNotifications = new List<UserNotificationRequest>();
            }
        }

        private async Task MarkNotificationAsReadAsync(UserNotificationRequest notification)
        {
            if (notification == null)
                return;
            MarkNotifications.Add(notification);
            await MarkNotificationsAsReadAsync(MarkNotifications);
        }

        private async Task MarkAllNotificationAsReadAsync()
        {
            MarkNotifications = UserNotifications.Where(n => n.IsRead == false).ToList();
            await MarkNotificationsAsReadAsync(MarkNotifications);
        }

        private async Task LoadUserNotificationsAsync()
        {
            if (_isLoading)
                return;

            try
            {
                _isLoading = true;
                var response = await UserNotificationService.GetAllAsync();

                if (response?.Success == true && response.Data != null)
                {
                    if (response.Data.Value != null)
                        UserNotifications = response.Data.Value.OrderByDescending(n => n.CreatedDateUtc).ToList() ?? new List<UserNotificationRequest>();
                    UnReadCount = response.Data.UnReadCount;
                }
                else
                {
                    // Silently fail - notifications are not critical
                    UserNotifications = new List<UserNotificationRequest>();
                    UnReadCount = 0;
                }
            }
            catch (Exception)
            {
                // Silently handle errors - notifications are not critical
                // Log the error if needed but don't disrupt user experience
                UserNotifications = new List<UserNotificationRequest>();
                UnReadCount = 0;
            }
            finally
            {
                _isLoading = false;
            }
        }

        private IOrderedEnumerable<IGrouping<string, UserNotificationRequest>> GroupedNotifications =>
           UserNotifications.GroupBy(n =>
               n.CreatedDateUtc.Date == DateTime.UtcNow.Date ? "Today" :
               n.CreatedDateUtc.Date == DateTime.UtcNow.AddDays(-1).Date ? "Yesterday" :
               "Earlier")
           .OrderBy(g => g.Key == "Today" ? 0 : g.Key == "Yesterday" ? 1 : 2);

        private void ShowAllNotifications()
        {
            NavigationManager.NavigateTo("/notifications", true);
        }
    }
}