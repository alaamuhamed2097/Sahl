using Dashboard.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Pages
{
    public partial class Home
    {
        // Lazy loading state
        private bool isInitialLoading = true;
        private bool loadAdminWidget = false;

        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IJSRuntime JS { get; set; } = null!;
        [Inject] protected CookieAuthenticationStateProvider CookieAuthenticationStateProvider { get; set; } = null!;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !isInitialLoading)
            {
                // Preload critical resources
                try
                {
                    await JS.InvokeVoidAsync("preloadCriticalResources");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error preloading resources: {ex.Message}");
                }
            }
        }

        protected async Task LoadAdminWidget()
        {
            if (!loadAdminWidget)
            {
                loadAdminWidget = true;
                StateHasChanged();
            }
        }

        protected async Task LogOut()
        {
            await CookieAuthenticationStateProvider.MarkUserAsLoggedOut();
            Navigation.NavigateTo("/login", true);
        }
    }
}
