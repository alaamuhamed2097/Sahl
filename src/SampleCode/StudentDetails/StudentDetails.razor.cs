using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Resources.Enumerations;
using Resources.Services;

namespace Dashboard.Pages.User.UserDetails
{
    public partial class StudentDetails : IDisposable
    {
        [Parameter] public Guid Id { get; set; } = Guid.Empty;

        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected LanguageService LanguageService { get; set; } = null!;

        private string ActiveTab { get; set; } = "basicInfoTab";

        protected override async Task OnInitializedAsync()
        {
            LanguageService.OnLanguageChanged += HandleLanguageChanged;
            await InitializeLanguageFromStorage();
        }

        private void SetActiveTab(string tabName)
        {
            ActiveTab = tabName;
            StateHasChanged();
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

        public void Dispose()
        {
            LanguageService.OnLanguageChanged -= HandleLanguageChanged;
        }
    }
}
