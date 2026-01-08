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
        //private string currentUrl;

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
            // امسح القوائم القديمة أولًا
            _openSubmenus.Clear();

            //var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Trim('/');
            var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri ?? string.Empty);

            // =========================
            // Admin / E-Commerce
            // =========================
            if (StartsWithAny(relativePath, new[] { "product", "category", "brand", "attribute", "unit", "shippingCompany" }))
                _openSubmenus.Add("catalog");

            if (StartsWithAny(relativePath, new[] { "users/administrators", "users/vendors" }))
                _openSubmenus.Add("users-management");

            if (StartsWithAny(relativePath, new[] { "promocode", "testimonial" }))
                _openSubmenus.Add("marketing");

            if (StartsWithAny(relativePath, new[] { "country", "state", "city" }))
                _openSubmenus.Add("shipping");

            if (StartsWithAny(relativePath, new[] { "content/pages" }))
                _openSubmenus.Add("content");


			if (StartsWithAny(relativePath, new[] { "ReviewManagement" }))
				_openSubmenus.Add("ReviewManagement");

			if (StartsWithAny(relativePath, new[] { "HomePageSlider" }))
				_openSubmenus.Add("HomePageSlider");

			if (StartsWithAny(relativePath, new[] { "settings", "currencies" }))
                _openSubmenus.Add("settings");


            //=========================
            //Vendor
            //=========================

            //if (StartsWithAny(relativePath, new[] { "commissions", "VendorBusinessPoints", "top-earners", "myTeam" }))
            //    _openSubmenus.Add("Vendor-earnings-network");

            //if (StartsWithAny(relativePath, new[] { "VendorOrders", "VendorGifts", "Vendor-events" }))
            //    _openSubmenus.Add("vendor-orders-rewards");

            //if (StartsWithAny(relativePath, new[] { "walletinfo", "VendorPaymentMethods" }))
            //    _openSubmenus.Add("vendor-wallet-payments");

            //if (StartsWithAny(relativePath, new[] { "PersonalInfo", "accountDetails", "VendorSettings" }))
            //    _openSubmenus.Add("vendor-account-settings");
            InvokeAsync(StateHasChanged);

        }
        // ميثود مساعدة لفحص البادئات
        private static bool StartsWithAny(string path, IEnumerable<string> prefixes)
        {
            foreach (var prefix in prefixes)
            {
                if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        // ميثود مساعدة لفحص البادئات
        //private static bool StartsWithAny(string path, IEnumerable<string> prefixes)
        //{
        //	foreach (var prefix in prefixes)
        //	{
        //		if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        //			return true;
        //	}
        //	return false;
        //}

        //private void EnsureSubmenuForCurrentRoute()
        //{
        //	// امسح القوائم القديمة أولًا
        //	_openSubmenus.Clear();

        //	var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Trim('/');

        //	// هنا تحدد القوائم الفرعية اللي تتفتح حسب الـ Route
        //	if (relativePath.StartsWith("products") || relativePath.StartsWith("categories"))
        //		_openSubmenus.Add("catalog");

        //	if (relativePath.StartsWith("users"))
        //		_openSubmenus.Add("users-management");

        //	if (relativePath.StartsWith("marketing"))
        //		_openSubmenus.Add("marketing");

        //	if (relativePath.StartsWith("settings") || relativePath.StartsWith("currencies"))
        //		_openSubmenus.Add("settings");

        //	// وأي submenu آخر حسب حاجتك
        //}


        //private void EnsureSubmenuForCurrentRoute()
        //{
        //    var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri ?? string.Empty);
        //    var trimmed = relativePath.Trim('/');
        //    var submenu = GetSubmenuForPath(trimmed);
        //    if (!string.IsNullOrWhiteSpace(submenu))
        //    {
        //        _openSubmenus.Add(submenu);
        //    }
        //}

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

        //private static bool StartsWithAny(string path, IEnumerable<string> prefixes)
        //{
        //    foreach (var prefix in prefixes)
        //    {
        //        if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}