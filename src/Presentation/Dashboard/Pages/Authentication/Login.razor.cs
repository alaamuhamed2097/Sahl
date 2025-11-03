using Dashboard.Contracts.CMS;
using Dashboard.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using System.Diagnostics.CodeAnalysis;

namespace Dashboard.Pages.Authentication
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public partial class Login : ComponentBase
    {
        private readonly LoginRequestModel _model = new();
        private string _errorMessage = string.Empty;
        private bool _isSubmitting;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; } = null!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter]
        public string? ReturnUrl { get; set; }

        public Login()
        {
        }

        protected void HandleForgetPassword()
        {
            // Navigate to forget password page or show modal
            NavigationManager.NavigateTo("/forget-password");
        }

        protected override async Task OnInitializedAsync()
        {
            await InitializeLanguageFromStorage();
        }

        private async Task InitializeLanguageFromStorage()
        {
            try
            {
                var lang = await JSRuntime.InvokeAsync<string>("localization.getCurrentLanguage");
                ResourceManager.CurrentLanguage = lang.StartsWith("ar") ? Language.Arabic : Language.English;
            }
            catch
            {
                // Fallback to English if localStorage is not available
                ResourceManager.CurrentLanguage = Language.English;
            }
        }

        private async Task HandleValidSubmit()
        {
            if (_isSubmitting)
                return;

            _isSubmitting = true;
            _errorMessage = string.Empty;
            StateHasChanged();

            try
            {
                var result = await AuthenticationService.Login(_model);

                if (result.Success && result.Data != null)
                {
                    // ? Wait for auth state to propagate
                    await Task.Delay(150);

                    // Determine redirect URL
                    var redirectUrl = string.Empty;
                    redirectUrl = string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl;

                    // ? Don't use forceLoad - prevents auth state reset
                    NavigationManager.NavigateTo(redirectUrl, forceLoad: false);
                    return;
                }

                _errorMessage = result.Message == "User not found." ? UserResources.UserNotFound : result.Message;
            }
            catch (Exception ex)
            {
                _errorMessage = NotifiAndAlertsResources.SomethingWentWrong;
                Console.Error.WriteLine($"Login error: {ex}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
