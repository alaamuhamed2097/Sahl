using Dashboard.Contracts.Currency;
using Dashboard.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Currency;

namespace Dashboard.Pages.Currency
{
    public partial class Details : LocalizedComponentBase
    {
        protected bool isSaving { get; set; }
        protected CurrencyDto Model { get; set; } = new();

        [Parameter] public Guid Id { get; set; }

        [Inject] protected ICurrencyService CurrencyService { get; set; } = null!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                await Edit(Id);
            }
        }

        protected async Task OnBaseCurrencyChanged(ChangeEventArgs e)
        {
            var isBaseCurrency = (bool)(e.Value ?? false);
            if (isBaseCurrency)
            {
                Model.ExchangeRate = 1m;
                Model.IsActive = true; // Base currency must be active
            }
            StateHasChanged();
        }

        protected async Task Save()
        {
            try
            {
                isSaving = true;
                StateHasChanged();

                // Ensure base currency has rate of 1
                if (Model.IsBaseCurrency)
                {
                    Model.ExchangeRate = 1m;
                }

                var result = await CurrencyService.SaveAsync(Model);

                if (result.Success)
                {
                    // If this is set as base currency, update all others
                    if (Model.IsBaseCurrency)
                    {
                        await CurrencyService.SetBaseCurrencyAsync(Model.Id);
                    }

                    await CloseModal();
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Done, "Currency saved successfully", "success");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, "Failed to save currency", "error");
                }
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, "An error occurred while saving", "error");
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
            }
        }

        protected async Task Edit(Guid id)
        {
            try
            {
                var result = await CurrencyService.GetByIdAsync(id);

                if (!result.Success || result.Data == null)
                {
                    await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Failed, "Currency not found", "error");
                    await CloseModal();
                    return;
                }

                Model = result.Data;
                StateHasChanged();
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, "Failed to load currency", "error");
                await CloseModal();
            }
        }

        protected async Task CloseModal()
        {
            Navigation.NavigateTo("/currencies");
        }
    }
}