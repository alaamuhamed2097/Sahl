using Dashboard.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources.Services;

namespace Dashboard.Layout
{
    public partial class NavMenu : IDisposable
    {
        private HashSet<string> _openSubmenus = new HashSet<string>();
        private string ComponentId = Guid.NewGuid().ToString("N")[..8];
        protected bool IsCollapsed = false;
        protected string baseUrl = string.Empty;
        protected string _ecommerceUrl = "";

        private string NavMenuClass => IsCollapsed ? "pcoded-navbar menupos-fixed navbar-collapsed" : "pcoded-navbar menupos-fixed";
        private string MobileMenuClass => IsCollapsed ? "mobile-menu on" : "mobile-menu";

        [Inject] private LanguageService LanguageService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            NavigationManager.LocationChanged += OnLocationChanged;
            LanguageService.OnLanguageChanged += HandleLanguageChanged;

            // Ensure correct submenu is opened based on current route
            EnsureSubmenuForCurrentRoute();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initializePcodedMenu");
                await JSRuntime.InvokeVoidAsync("setActiveMenuItems");
                await JSRuntime.InvokeVoidAsync("initializePerfectScrollbar", $"layout-sidenav-{ComponentId}");
            }
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            IsCollapsed = false;
            // Open the submenu corresponding to the current route
            EnsureSubmenuForCurrentRoute();
            InvokeAsync(async () =>
            {
                // Clear all previous active states and set new ones
                await JSRuntime.InvokeVoidAsync("clearAllActiveStates");
                await JSRuntime.InvokeVoidAsync("setActiveMenuItems");
                StateHasChanged();
            });
        }

        private void HandleLanguageChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            LanguageService.OnLanguageChanged -= HandleLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        private void ToggleSubmenu(string submenuName)
        {
            if (_openSubmenus.Contains(submenuName))
            {
                _openSubmenus.Remove(submenuName);
            }
            else
            {
                _openSubmenus.Add(submenuName);
            }
            StateHasChanged();
        }

        private bool IsSubmenuOpen(string submenuName)
        {
            return _openSubmenus.Contains(submenuName);
        }

        private void EnsureSubmenuForCurrentRoute()
        {
            var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri ?? string.Empty);
            var trimmed = relativePath.Trim('/');
            var submenu = GetSubmenuForPath(trimmed);
            if (!string.IsNullOrWhiteSpace(submenu))
            {
                _openSubmenus.Add(submenu);
            }
        }

        private static string? GetSubmenuForPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return null;

            if (StartsWithAny(relativePath, new[] { "commissions", "VendorBusinessPoints", "top-earners", "myTeam" }))
                return "Vendor-earnings-network";

            if (StartsWithAny(relativePath, new[] { "VendorOrders", "VendorGifts", "Vendor-events" }))
                return "Vendor-orders-rewards";

            if (StartsWithAny(relativePath, new[] { "walletinfo", "VendorPaymentMethods" }))
                return "Vendor-wallet-payments";

            if (StartsWithAny(relativePath, new[] { "PersonalInfo", "accountDetails", "VendorSettings" }))
                return "Vendor-account-settings";

            return null;
        }

        private static bool StartsWithAny(string path, IEnumerable<string> prefixes)
        {
            foreach (var prefix in prefixes)
            {
                if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}