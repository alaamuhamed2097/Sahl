using Dashboard.Constants;
using Dashboard.Contracts.Location;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Pages.Location.City
{
    public partial class Index : BaseListPage<CityDto>
    {
        protected override string EntityName { get; } = "City";
        protected override string AddRoute { get; } = "/city";
        protected override string EditRouteTemplate { get; } = $"/city/{{id}}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.City.Search;
        protected override Dictionary<string, Func<CityDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<CityDto, object>>
        {
            [ECommerceResources.Title] = x => x.Title,
        };

        [Inject] protected ICityService CityService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<CityDto>>> GetAllItemsAsync()
        {
            var result = await CityService.GetAllAsync();
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
            return await CityService.DeleteAsync(id);
        }
        protected override async Task<string> GetItemId(CityDto item)
        {
            var result = await CityService.GetByIdAsync(item.Id);
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
