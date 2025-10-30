using Dashboard.Contracts.Notification;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Components.Notifications
{
    public partial class NotificationsDetails : IAsyncDisposable
    {
        private string ComponentId = Guid.NewGuid().ToString("N")[..8];
        private bool IsLoading = false;
        private bool HasMoreData = true;
        private int unReadCount { get; set; }
        private int totalCount { get; set; }
        protected List<UserNotificationRequest> UserNotifications = new();
        protected List<UserNotificationRequest> MarkNotifications { get; set; } = new List<UserNotificationRequest>();

        protected BaseSearchCriteriaModel searchModel { get; set; } = new();
        protected int currentPage = 1;
        protected int totalRecords = 10;
        protected int totalPages = 1;

        [Parameter] public EventCallback<bool> OnNotificationRead { get; set; }
        [Inject] private IUserNotificationService UserNotificationService { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadInitialNotificationsAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("initializePerfectScrollbar", $"notifications-content-{ComponentId}");
                    // Initialize scroll event listener for lazy loading
                    await JSRuntime.InvokeVoidAsync("initializeScrollListener",
                        $"notifications-content-{ComponentId}",
                        DotNetObjectReference.Create(this));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing scrollbar: {ex.Message}");
                }
            }
        }
        private async Task LoadInitialNotificationsAsync()
        {
            currentPage = 1;
            HasMoreData = true;
            UserNotifications.Clear();
            await LoadMoreNotificationsAsync();
        }

        private async Task LoadMoreNotificationsAsync()
        {
            if (IsLoading || !HasMoreData)
                return;

            IsLoading = true;
            StateHasChanged();

            try
            {
                searchModel.PageNumber = currentPage;
                var response = await UserNotificationService.SearchAsync(searchModel);
                if (response.Success && response.Data.Value != null)
                {
                    var newNotifications = response.Data.Value.Items
                        .OrderByDescending(n => n.CreatedDateUtc)
                        .ToList();
                    unReadCount = response.Data.UnReadCount;
                    totalCount = response.Data.TotalCount;
                    StateHasChanged();

                    //HashSet for faster lookup
                    var existingIds = new HashSet<Guid>(UserNotifications.Select(n => n.Id));

                    foreach (var notification in newNotifications)
                    {
                        if (!existingIds.Contains(notification.Id))
                        {
                            UserNotifications.Add(notification);
                        }
                        if (!notification.IsRead)
                            MarkNotifications.Add(notification);
                    }
                    if (MarkNotifications.Count > 0)
                        await MarkNotificationsAsReadAsync(MarkNotifications);
                    totalRecords = response.Data.Value.TotalRecords;
                    totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);

                    // FIXED: Check if we have more data to load
                    HasMoreData = currentPage < totalPages;

                    // FIXED: Only increment if we successfully loaded data
                    if (newNotifications.Any())
                    {
                        currentPage++;
                    }
                    else
                    {
                        HasMoreData = false; // No more data if empty response
                    }
                }
                else
                {
                    HasMoreData = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading notifications: {ex.Message}");
                HasMoreData = false;
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
                await UpdateScrollbar();
            }
        }

        [JSInvokable]
        public async Task OnScrollNearEnd()
        {
            await LoadMoreNotificationsAsync();
        }

        private async Task MarkNotificationsAsReadAsync(IEnumerable<UserNotificationRequest> markNotifications)
        {
            if (MarkNotifications == null || MarkNotifications.Count() == 0)
                return;

            var response = await UserNotificationService.MarkAsReadAsync(MarkNotifications);
            if (response.Success)
            {
                // Update the local list instead of reloading everything
                foreach (var notification in MarkNotifications)
                {
                    UserNotifications.First(n => n.Id == notification.Id).IsRead = true;
                }
                unReadCount = response.Data.UnReadCount;
                totalCount = response.Data.TotalCount;
                StateHasChanged();
                await UpdateScrollbar();

                // Notify parent component about read notifications
                await OnNotificationRead.InvokeAsync(true);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed, "error");
            }
            MarkNotifications = new List<UserNotificationRequest>();
        }

        private async Task MarkAllNotificationAsReadAsync()
        {
            MarkNotifications = UserNotifications.Where(n => n.IsRead == false).ToList();
            await MarkNotificationsAsReadAsync(MarkNotifications);
            StateHasChanged();
            await UpdateScrollbar();
        }
        private async Task RefreshNotificationsAsync()
        {
            await LoadInitialNotificationsAsync();
        }
        private async Task UpdateScrollbar()
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("updatePerfectScrollbar", $"notifications-content-{ComponentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating scrollbar: {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("removeScrollListener", $"notifications-content-{ComponentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing scroll listener: {ex.Message}");
            }
        }
    }
}