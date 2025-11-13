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

        protected override async Task OnInitializedAsync()
        {
            // Simulate initial loading
            await Task.Delay(500);
            isInitialLoading = false;
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !isInitialLoading)
            {
                // Preload critical resources and initialize charts
                try
                {
                    await Task.Delay(300);
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

        protected async Task LoadAdminWidget()
        {
            if (!loadAdminWidget)
            {
                loadAdminWidget = true;
                StateHasChanged();

                // Wait for the component to render and then initialize charts
                await Task.Delay(200);
                try
                {
                    await JS.InvokeVoidAsync("eval", "if (typeof floatchart === 'function') { floatchart(); }");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading admin widget: {ex.Message}");
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
