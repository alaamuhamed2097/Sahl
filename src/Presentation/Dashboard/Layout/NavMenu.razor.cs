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

        // =========================
        // Toggle Submenu Methods
        // =========================

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

        // Individual toggle methods for each submenu
        private void ToggleCatalogSubmenu() => ToggleSubmenu("catalog");
        private void ToggleOrdersSubmenu() => ToggleSubmenu("orders");
        private void ToggleUsersManagementSubmenu() => ToggleSubmenu("users-management");
        private void ToggleLogisticsSubmenu() => ToggleSubmenu("logistics");
        private void ToggleMerchandisingSubmenu() => ToggleSubmenu("merchandising");
        private void ToggleContentSubmenu() => ToggleSubmenu("content");
        private void ToggleSettingsSubmenu() => ToggleSubmenu("settings");
        private void ToggleVendorOrdersRewardsSubmenu() => ToggleSubmenu("vendor-orders-rewards");
        private void ToggleVendorAccountSettingsSubmenu() => ToggleSubmenu("vendor-account-settings");

        private bool IsSubmenuOpen(string submenuName)
        {
            return _openSubmenus.Contains(submenuName);
        }

        private void EnsureSubmenuForCurrentRoute()
        {
            // Clear old submenus first
            _openSubmenus.Clear();

            var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri ?? string.Empty);

            // =========================
            // ADMIN MENU ROUTES
            // =========================

            // 📦 Catalog Management
            if (StartsWithAny(relativePath, new[] { "products", "categories", "brands", "attributes", "units" }))
                _openSubmenus.Add("catalog");

            // 🛒 Orders
            if (StartsWithAny(relativePath, new[] { "sales/orders", "refunds" }))
                _openSubmenus.Add("orders");

            // 👥 User Management
            if (StartsWithAny(relativePath, new[] { "users/administrators", "users/vendors", "users/customers" }))
                _openSubmenus.Add("users-management");

            // 🚚 Logistics & Shipping
            if (StartsWithAny(relativePath, new[] { "warehouses", "shippingCompanies", "countries", "states", "cities" }))
                _openSubmenus.Add("logistics");

            // 📢 Merchandising
            if (StartsWithAny(relativePath, new[] { "campaigns", "couponCodes", "HomepageSlider", "home-blocks" }))
                _openSubmenus.Add("merchandising");

            // 🎨 Content & CMS
            if (StartsWithAny(relativePath, new[] { "content/pages", "content/areas", "content/media" }))
                _openSubmenus.Add("content");

            // ⚙️ Settings & Configuration
            if (StartsWithAny(relativePath, new[] { "settings", "system-settings" }))
                _openSubmenus.Add("settings");

            // =========================
            // VENDOR MENU ROUTES
            // =========================

            // 📦 Orders & Rewards
            if (StartsWithAny(relativePath, new[] { "vendor/orders" }))
                _openSubmenus.Add("vendor-orders-rewards");

            // 👤 Account & Settings
            if (StartsWithAny(relativePath, new[] { "vendor/personal-info", "vendor/account-details", "vendor/settings" }))
                _openSubmenus.Add("vendor-account-settings");

            InvokeAsync(StateHasChanged);
        }

        // Helper method to check prefixes
        private static bool StartsWithAny(string path, IEnumerable<string> prefixes)
        {
            foreach (var prefix in prefixes)
            {
                if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}