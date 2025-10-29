using Microsoft.AspNetCore.Components;
using Dashboard.Contracts.Handlers;

namespace Dashboard.Handlers
{
    public class ApiStatusHandler : IApiStatusHandler
    {
        private readonly NavigationManager _navigationManager;
        public ApiStatusHandler(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void HandleUnAuthorizeStatusAsync()
        {
            _navigationManager.NavigateTo("/login", true);
        }
    }
}
