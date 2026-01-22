using Dashboard.Contracts.CMS;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.User;

namespace Dashboard.Pages.Authentication
{
    public partial class ForgotPassword : LocalizedComponentBase
    {
        private readonly ForgetPasswordRequestDto _model = new();
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;
        private bool _isSubmitting;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; } = null!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

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
                ResourceManager.CurrentLanguage = Language.English;
            }
        }

        private async Task HandleValidSubmit()
        {
            if (_isSubmitting)
                return;

            _isSubmitting = true;
            _errorMessage = string.Empty;
            _successMessage = string.Empty;
            StateHasChanged();

            try
            {
                var result = await AuthenticationService.SendResetPasswordCode(_model.Identifier);
                if (result.Success)
                {
                    _successMessage = result.Message ?? ValidationResources.PasswordResetCodeSent;
                    // Store email in localStorage then redirect
                    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "resetEmail", _model.Identifier);
                    await Task.Delay(1000); // Optional: short delay to show success message
                    NavigationManager.NavigateTo("/reset-password");
                }
                else
                {
                    _errorMessage = result.Message ?? ValidationResources.PasswordResetCodeFailed;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = NotifiAndAlertsResources.SomethingWentWrong;
                Console.Error.WriteLine($"ForgotPassword error: {ex}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
