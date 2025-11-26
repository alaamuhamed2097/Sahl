using Dashboard.Contracts.General;
using Dashboard.Contracts.Warehouse;
using Dashboard.Services.General;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Warehouse;

namespace Dashboard.Pages.Warehouse
{
    public partial class Details
    {
        private bool isSaving { get; set; }
        private IEnumerable<CountryInfo>? countries;

        protected WarehouseDto Model { get; set; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected IWarehouseService WarehouseService { get; set; } = null!;
        [Inject] protected ICountryPhoneCodeService CountryPhoneCodeService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
            Model = new WarehouseDto { IsActive = true };
        }

        protected override void OnParametersSet()
        {
            if (Id != Guid.Empty)
            {
                Edit(Id);
            }
        }

        private async Task LoadCountriesAsync()
        {
            try
            {
                if (CountryPhoneCodeService != null)
                {
                    countries = CountryPhoneCodeService.GetAllCountries(
                        ResourceManager.CurrentLanguage == Resources.Enumerations.Language.Arabic ? "ar" : "en");
                    
                    if (string.IsNullOrEmpty(Model.PhoneCode))
                    {
                        Model.PhoneCode = "+20"; // Default to Egypt
                    }
                }
            }
            catch (Exception)
            {
                countries = CountryPhoneCodeService?.GetFallbackCountries();
            }
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged();

                var result = await WarehouseService.SaveAsync(Model);

                isSaving = false;
                if (result.Success)
                {
                    await ShowSuccessNotification(NotifiAndAlertsResources.SavedSuccessfully);
                    await CloseModal();
                }
                else
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.SaveFailed);
                }
            }
            catch (Exception)
            {
                await ShowErrorNotification(NotifiAndAlertsResources.FailedAlert, NotifiAndAlertsResources.SomethingWentWrong);
            }
        }

        protected async Task Edit(Guid id)
        {
            try
            {
                var result = await WarehouseService.GetByIdAsync(id);

                if (!result.Success)
                {
                    await ShowErrorNotification(ValidationResources.Failed, NotifiAndAlertsResources.FailedToRetrieveData);
                    return;
                }

                Model = result.Data ?? new();
                StateHasChanged();
            }
            catch (Exception)
            {
                await ShowErrorNotification(ValidationResources.Error, NotifiAndAlertsResources.SomethingWentWrong);
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/warehouses", true);
        }

        private async Task ShowErrorNotification(string title, string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", title, message, "error");
        }

        private async Task ShowSuccessNotification(string message)
        {
            await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, message, "success");
        }
    }
}
