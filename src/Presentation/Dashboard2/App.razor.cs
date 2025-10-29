using Microsoft.AspNetCore.Components;

namespace Dashboard
{
    public partial class App
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        // ✅ REMOVED: OnNavigateAsync navigation guard
        // Let AuthorizeRouteView handle authentication on protected pages
        // This prevents redirect loops during login
    }
}