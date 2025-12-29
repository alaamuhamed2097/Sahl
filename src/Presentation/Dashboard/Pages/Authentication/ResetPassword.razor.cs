using Dashboard.Contracts.CMS;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.User;
using System.Diagnostics.CodeAnalysis;

namespace Dashboard.Pages.Authentication
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public partial class ResetPassword : ComponentBase
    {
        private readonly ResetPasswordWithCodeDto _model = new();
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;
        private bool _isSubmitting;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; } = null!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        [Inject]
        private NavigationManager NavManager { get; set; } = null!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? Identifier { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeLanguageFromStorage();
        }

        protected override void OnParametersSet()
        {
            // If Identifier is present in the query string, populate the model so validation can succeed
            if (!string.IsNullOrWhiteSpace(Identifier))
            {
                _model.Identifier = Identifier!;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // On first render, try to pull the Identifier from localStorage so the model is valid before submit
            if (firstRender && string.IsNullOrEmpty(_model.Identifier))
            {
                try
                {
                    var IdentifierFromCache = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "resetIdentifier");
                    if (!string.IsNullOrEmpty(IdentifierFromCache))
                    {
                        _model.Identifier = IdentifierFromCache;
                        StateHasChanged();
                    }
                }
                catch { }
            }
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
                // Ensure Identifier is present; if not, redirect user to request reset again
                if (string.IsNullOrEmpty(_model.Identifier))
                {
                    NavigationManager.NavigateTo("/forget-password");
                    return;
                }

                var result = await AuthenticationService.ResetPasswordWithCode(_model);
                if (result.Success)
                {
                    _successMessage = result.Message ?? ValidationResources.PasswordResetSuccess;
                    await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "resetIdentifier");
                    await Task.Delay(1000);
                    NavigationManager.NavigateTo("/login");
                }
                else
                {
                    if (result.Message == "Too many attempts")
                    {
                        _errorMessage = "لقد تجاوزت عدد المحاولات المسموح بها. الرجاء طلب كود جديد.";
                    }
                    else
                    {
                        _errorMessage = result.Message ?? ValidationResources.PasswordResetFailed;
                    }
                }
            }
            catch (Exception)
            {
                _errorMessage = NotifiAndAlertsResources.SomethingWentWrong;
                // يمكن إضافة لوج هنا إذا أردت تتبع الأخطاء
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
