using Common.Enumerations.User;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Resources.Services;
using Shared.DTOs.User.Marketer;
using UI.Configuration;
using UI.Contracts.User.Marketer;

namespace UI.Pages.User.Marketer.Details
{
    public partial class MarketerDetails
    {
        [Parameter] public string id { get; set; }
        [Inject] protected IMarketerService MarketerService { get; set; } = null!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IOptions<ApiSettings> ApiOptions { get; set; } = default!;
        [Inject] protected LanguageService LanguageService { get; set; } = null!;

        protected string baseUrl = string.Empty;
        protected MarketerDto Model { get; set; } = new();
        protected SponsorDto Sponsor { get; set; } = new();
        protected bool IsLoadingTransactions { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            LanguageService.OnLanguageChanged += HandleLanguageChanged;
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

        private void HandleLanguageChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        protected override async Task OnParametersSetAsync()
        {
            baseUrl = ApiOptions.Value.BaseUrl;

            Guid userId = Guid.Empty;
            Guid.TryParse(id, out userId);

            var result = await MarketerService.FindById(userId);
            if (!result.Success || result.Data == null)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData, "error");
                return; // Exit immediately after navigation
            }
            //Navigation.NavigateTo("/NotFoundPage");

            Model = result.Data;

        }

        protected async Task HandleChangeUserState(UserStateType newType)
        {
            var result = await MarketerService.ChangeMarketerStates(Model.MarketerId, newType);
            if (!result.Success || result?.Data == null)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, NotifiAndAlertsResources.Failed, "error");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, result.Message, "success");
            }

            Model.UserState = newType;
            StateHasChanged();
        }

        public void Dispose()
        {
            LanguageService.OnLanguageChanged -= HandleLanguageChanged;
        }
    }
}