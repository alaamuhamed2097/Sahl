using Dashboard.Contracts.General;
using Dashboard.Models;
using Microsoft.AspNetCore.Components;

namespace Dashboard.Pages.Auth
{
    public partial class Login : ComponentBase
    {
        private readonly LoginRequestModel _model = new();
        private string _errorMessage = string.Empty;
        private bool _isSubmitting;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; } = null!;

        [Parameter]
        public string? ReturnUrl { get; set; }

        private async Task HandleValidSubmit()
        {
            if (_isSubmitting)
                return;

            _isSubmitting = true;
            _errorMessage = string.Empty;

            try
            {
                var result = await AuthenticationService.Login(
                    _model);

                if (result.Success)
                {
                    var redirectUrl = string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl;
                    NavigationManager.NavigateTo(redirectUrl, forceLoad: false);
                    return;
                }

                _errorMessage = result.Message ?? "Invalid login attempt";
            }
            catch (Exception ex)
            {
                _errorMessage = "An unexpected error occurred during login";
                // In production, log this error (e.g., ILogger)
                Console.Error.WriteLine($"Login error: {ex}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
