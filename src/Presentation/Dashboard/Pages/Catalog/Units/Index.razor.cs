using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.ECommerce.Unit;
using Shared.GeneralModels;

namespace Dashboard.Pages.Catalog.Units
{
    public partial class Index : BaseListPage<UnitDto>
    {
        protected override string EntityName { get; } = ECommerceResources.Units;
        protected override string AddRoute { get; } = $"/unit";
        protected override string EditRouteTemplate { get; } = "/unit/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Unit.Search;
        protected override Dictionary<string, Func<UnitDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<UnitDto, object>>
        {
            [ECommerceResources.Title] = x => x.Title,
        };
        [Inject] protected IUnitService UnitService { get; set; } = null!;
        protected override async Task<ResponseModel<IEnumerable<UnitDto>>> GetAllItemsAsync()
        {
            var result = await UnitService.GetAllAsync();
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
            return await UnitService.DeleteAsync(id);
        }
        protected override async Task<string> GetItemId(UnitDto item)
        {
            var result = await UnitService.GetByIdAsync(item.Id);
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
