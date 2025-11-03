using Dashboard.Contracts.Currency;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Currency;
using Shared.GeneralModels;

namespace Dashboard.Pages.Settings.Currencies
{
    public partial class Index : BaseListPage<CurrencyDto>
    {
        [Inject] private ICurrencyService CurrencyService { get; set; } = null!;

        private bool _isUpdatingRates = false;
        private string _statusFilter = string.Empty;

        // Property with custom setter that calls ApplyFilters
        protected string StatusFilter
        {
            get => _statusFilter;
            set
            {
                _statusFilter = value;
                _ = ApplyFilters(); // Fire and forget the async method
            }
        }

        // Properties to match the BaseListPage pattern
        protected override string EntityName => CurrencyResources.CurrencyManagement;
        protected override string AddRoute => $"/settings/currencies/{Guid.Empty}";
        protected override string EditRouteTemplate => "/settings/currencies/{id}";
        protected override string SearchEndpoint => "api/Currency/search";

        protected override Dictionary<string, Func<CurrencyDto, object>> ExportColumns => new()
        {
            { CurrencyResources.Code, c => c.Code },
            { $"{FormResources.ArabicName}", c => c.NameAr },
            { $"{FormResources.EnglishName}", c => c.NameEn },
            { CurrencyResources.Symbol, c => c.Symbol },
            { CurrencyResources.ExchangeRate, c => c.ExchangeRate.ToString("N2") },
            { FormResources.Status, c => c.IsActive ? CurrencyResources.Active : CurrencyResources.Inactive },
            { $"{GeneralResources.All} {CurrencyResources.Base}", c => c.IsBaseCurrency ? GeneralResources.All : GeneralResources.All },
            { GeneralResources.Country, c => c.CountryCode ?? "" }
        };

        protected override async Task<ResponseModel<IEnumerable<CurrencyDto>>> GetAllItemsAsync()
        {
            return await CurrencyService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await CurrencyService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(CurrencyDto item)
        {
            return await Task.FromResult(item.Id.ToString());
        }

        // Renamed method to avoid conflict with base class
        protected async Task DeleteCurrency(CurrencyDto currency)
        {
            if (currency.IsBaseCurrency)
            {
                await ShowWarningNotification(
                    NotifiAndAlertsResources.Warning,
                    CurrencyResources.CannotDeleteBaseCurrency);
                return;
            }

            // the base class Delete method properly
            await base.Delete(currency.Id);
        }

        protected async Task SetBaseCurrency(CurrencyDto currency)
        {
            try
            {
                var confirmed = await ShowConfirmDialog(
                    CurrencyResources.SetBaseCurrency,
                    string.Format(CurrencyResources.SetBaseCurrencyConfirmation, currency.Code),
                    CurrencyResources.YesSetAsBase,
                    ActionsResources.Cancel
                );

                if (!confirmed) return;

                var result = await CurrencyService.SetBaseCurrencyAsync(currency.Id);
                if (result.Success)
                {
                    await ShowSuccessNotification(CurrencyResources.BaseCurrencyUpdatedSuccessfully);
                    await Search(); // Refresh the data
                }
                else
                {
                    await ShowErrorNotification(NotifiAndAlertsResources.Error, result.Message ?? CurrencyResources.FailedToSetBaseCurrency);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, $"{NotifiAndAlertsResources.Error}: {ex.Message}");
            }
        }

        protected async Task UpdateExchangeRates()
        {
            try
            {
                _isUpdatingRates = true;
                StateHasChanged();

                var result = await CurrencyService.UpdateExchangeRatesAsync();
                if (result.Success)
                {
                    await ShowSuccessNotification(CurrencyResources.ExchangeRatesUpdatedSuccessfully);
                    await Search(); // Refresh the data
                }
                else
                {
                    await ShowErrorNotification(NotifiAndAlertsResources.Error, result.Message ?? CurrencyResources.FailedToUpdateExchangeRates);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, $"{NotifiAndAlertsResources.Error}: {ex.Message}");
            }
            finally
            {
                _isUpdatingRates = false;
                StateHasChanged();
            }
        }

        private async Task ApplyFilters()
        {
            // Apply status filter to the search
            if (!string.IsNullOrEmpty(_statusFilter))
            {
                // Add custom filter logic here
                // This would typically invoke modifying the search criteria
                // For now, we'll filter on the client side after getting results
            }

            await Search();
        }

        protected override async Task OnAfterSearchAsync()
        {
            // Apply client-side filtering if needed
            if (!string.IsNullOrEmpty(_statusFilter) && items != null)
            {
                items = _statusFilter.ToLower() switch
                {
                    "active" => items.Where(c => c.IsActive),
                    "inactive" => items.Where(c => !c.IsActive),
                    _ => items
                };
            }

            await base.OnAfterSearchAsync();
        }

        private async Task ClearFilters()
        {
            _statusFilter = string.Empty;
            searchModel.SearchTerm = string.Empty;
            await Search();
        }

        protected override async Task OnCustomInitializeAsync()
        {
            // Initialize with default page size
            searchModel.PageSize = 10;
            await base.OnCustomInitializeAsync();
        }

        protected override async Task<bool> OnBeforeDeleteAsync(Guid id)
        {
            var currency = items?.FirstOrDefault(c => c.Id == id);
            if (currency?.IsBaseCurrency == true)
            {
                await ShowWarningNotification(
                    NotifiAndAlertsResources.Warning,
                    CurrencyResources.CannotDeleteBaseCurrency);
                return false;
            }

            return await base.OnBeforeDeleteAsync(id);
        }
    }
}