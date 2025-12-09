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
    public partial class Register : ComponentBase
    {
        private readonly RegisterRequestModel _model = new();
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
                // Create DTO from model
                var registerDto = new Shared.DTOs.User.Customer.CustomerRegistrationDto
                {
                    FirstName = _model.FirstName,
                    LastName = _model.LastName,
                    Email = _model.Email,
                    PhoneNumber = _model.PhoneNumber,
                    PhoneCode = _model.PhoneCode,
                    Password = _model.Password,
                    ConfirmPassword = _model.ConfirmPassword
                };

                var result = await AuthenticationService.RegisterCustomer(registerDto);

                if (result.Success && result.Data != null)
                {
                    _successMessage = NotifiAndAlertsResources.RegistrationSuccessful;
                    StateHasChanged();

                    // Wait a bit for user to see the success message
                    await Task.Delay(1500);

                    // Redirect to login
                    NavigationManager.NavigateTo("/login", forceLoad: false);
                    return;
                }

                _errorMessage = result.Message ?? NotifiAndAlertsResources.RegistrationFailed;
            }
            catch (Exception ex)
            {
                _errorMessage = NotifiAndAlertsResources.SomethingWentWrong;
                Console.Error.WriteLine($"Registration error: {ex}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private async Task LoginWithGoogle()
        {
            try
            {
                _isSubmitting = true;
                // Initialize Google OAuth flow
                await JSRuntime.InvokeVoidAsync("googleOAuth.startGoogleSignIn");
            }
            catch (Exception ex)
            {
                _errorMessage = "Failed to initialize Google login";
                Console.Error.WriteLine($"Google login error: {ex}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private async Task LoginWithFacebook()
        {
            try
            {
                _isSubmitting = true;
                // Initialize Facebook OAuth flow
                await JSRuntime.InvokeVoidAsync("facebookOAuth.startFacebookSignIn");
            }
            catch (Exception ex)
            {
                _errorMessage = "Failed to initialize Facebook login";
                Console.Error.WriteLine($"Facebook login error: {ex}");
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
