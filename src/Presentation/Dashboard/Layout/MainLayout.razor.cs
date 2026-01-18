using Dashboard.Configuration;
using Dashboard.Contracts.Notification;
using Dashboard.Contracts.Setting;
using Dashboard.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Resources.Services;

namespace Dashboard.Layout
{
    [Authorize]
    public partial class MainLayout : LayoutComponentBase
    {
        protected bool NotificationChanged { get; set; } = false;
        protected string baseUrl = string.Empty;
        protected string UserImage { get; set; } = "Images/ProfileImages/default.png";
        protected string fullName { get; set; } = "User Name";

        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        [Inject] protected INotificationStateService NotificationStateService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected LanguageService LanguageService { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected CookieAuthenticationStateProvider CookieAuthenticationStateProvider { get; set; } = null!;
        [Inject] protected ISettingService SettingService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            fullName = user.Claims.FirstOrDefault(c => c.Type == "fullName")?.Value ?? "User Name";
            UserImage = user.Claims.FirstOrDefault(c => c.Type == "userImage")?.Value ?? UserImage;
            if (user?.Identity is null || (!user.Identity.IsAuthenticated && !Navigation.Uri.EndsWith("/login")))
            {
                Navigation.NavigateTo("/login", forceLoad: false);
            }

            // Subscribe to events
            LanguageService.OnLanguageChanged += HandleLanguageChanged;
            NotificationStateService.OnNotificationRead += HandleNotificationChanged;

            // Initialize language from localStorage
            await InitializeLanguageFromStorage();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("navbarHandler.init");
            }
        }

        protected async Task LogOut()
        {
            await CookieAuthenticationStateProvider.MarkUserAsLoggedOut();
            Navigation.NavigateTo("/login", true);
        }

        private void HandleLanguageChanged()
        {
            // Force component re-render
            InvokeAsync(StateHasChanged);
        }

        private async Task InitializeLanguageFromStorage()
        {
            var lang = await JSRuntime.InvokeAsync<string>("localization.getCurrentLanguage");
            ResourceManager.CurrentLanguage = lang.StartsWith("ar") ? Language.Arabic : Language.English;
        }

        private async Task ChangeLanguage()
        {
            ResourceManager.ChangeLanguage();
            var languageCode = ResourceManager.GetCultureName(ResourceManager.CurrentLanguage);
            await JSRuntime.InvokeVoidAsync("localization.setLanguage", languageCode);
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }

        // Handler for notification received event
        private void HandleNotificationChanged(bool hasNewNotification)
        {
            NotificationChanged = hasNewNotification;
            InvokeAsync(StateHasChanged);

            // Reset the flag after a short delay to allow the Notifications component to react
            _ = Task.Delay(100).ContinueWith(_ =>
            {
                InvokeAsync(() =>
                {
                    NotificationChanged = false;
                    StateHasChanged();
                });
            });
        }

        public void Dispose()
        {
            // Unsubscribe when component is removed
            LanguageService.OnLanguageChanged -= HandleLanguageChanged;
        }
    }
}