using Dashboard.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dashboard.Pages
{
    public partial class Home
    {
        // Lazy loading state
        private bool isInitialLoading = true;

        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IJSRuntime JS { get; set; } = null!;
        [Inject] protected CookieAuthenticationStateProvider CookieAuthenticationStateProvider { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            // Simulate initial loading with a minimal delay for optimal UX
            await Task.Delay(300);
            isInitialLoading = false;
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !isInitialLoading)
            {
                // Initialize charts after components are rendered
                try
                {
                    await Task.Delay(100);
                    await JS.InvokeVoidAsync("eval", @"
                        if (typeof floatchart === 'function') {
                            floatchart();
                        }
                    ");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing charts: {ex.Message}");
                }
            }
        }

        protected async Task LogOut()
        {
            await CookieAuthenticationStateProvider.MarkUserAsLoggedOut();
            Navigation.NavigateTo("/login", true);
        }
    }
}
