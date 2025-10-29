using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace Dashboard
{
    public partial class App
    {
        private bool isAuthorized = true;
        private bool isLoading = false;

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        [Inject] protected IAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            await CheckAuthenticationAsync();


            // Subscribe to navigation changes to re-check authentication
            Navigation.LocationChanged += OnLocationChanged;
        }

        private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            await CheckAuthenticationAsync();
            InvokeAsync(StateHasChanged);
        }

        private async Task CheckAuthenticationAsync()
        {
            isLoading = true;
            StateHasChanged();

            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user?.Identity is null || (!user.Identity.IsAuthenticated && !Navigation.Uri.EndsWith("/login")))
                    isAuthorized = await AuthenticationService.TryRefreshTokenAsync();
                if (!isAuthorized)
                    Navigation.NavigateTo("/login", forceLoad: true);
            }
            catch (Exception)
            {
                isAuthorized = false;
                Navigation.NavigateTo("/login", forceLoad: true);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}
