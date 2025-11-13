using Dashboard.Constants;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Pages.Location.Country
{
    public partial class Index : BaseListPage<CountryDto>
    {
        protected override string EntityName { get; } = "Country";
        protected override string AddRoute { get; } = "/country";
        protected override string EditRouteTemplate { get; } = $"/country/{{id}}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Country.Search;
        protected override Dictionary<string, Func<CountryDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<CountryDto, object>>
        {
            [ECommerceResources.Title] = x => x.Title,
        };

        [Inject] protected ICountryService CountryService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<CountryDto>>> GetAllItemsAsync()
        {
            var result = await CountryService.GetAllAsync();
            if (result.Success)
            {
                return result;
            }
            else
            {
                return result;
            }
        }
        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await CountryService.DeleteAsync(id);
        }
        protected override async Task<string> GetItemId(CountryDto item)
        {
            var result = await CountryService.GetByIdAsync(item.Id);
            if (result.Success)
            {
                return result.Data?.Id.ToString() ?? string.Empty;
            }
            else
            {
                await ShowErrorNotification(NotifiAndAlertsResources.Error, NotifiAndAlertsResources.NoDataFound);
                return string.Empty;
            }
        }
    }
}
