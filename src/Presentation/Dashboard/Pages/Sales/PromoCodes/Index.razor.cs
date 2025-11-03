using Dashboard.Constants;
using Dashboard.Contracts;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.ECommerce.PromoCode;
using Shared.GeneralModels;

namespace Dashboard.Pages.Sales.PromoCodes
{
    public partial class Index : BaseListPage<PromoCodeDto>
    {
        protected override string EntityName { get; } = FormResources.PromoCodes;
        protected override string AddRoute { get; } = $"/sales/promocodes/{Guid.Empty}";
        protected override string EditRouteTemplate { get; } = "/sales/promocodes/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.PromoCode.Search;
        protected override Dictionary<string, Func<PromoCodeDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<PromoCodeDto, object>>
        {
            [FormResources.ArabicTitle] = x => x.TitleAR,
            [FormResources.EnglishTitle] = x => x.TitleEN,
            [FormResources.Code] = x => x.Code,
            [FormResources.StartDate] = x => x.StartDate.ToString("yyyy-MM-dd"),
            [FormResources.EndDate] = x => x.EndDate.ToString("yyyy-MM-dd"),
            [FormResources.Type] = x => x.PromoCodeType,
            [FormResources.Value] = x => x.Value,
            [FormResources.UsageLimit] = x => x.UsageLimit?.ToString() ?? "∞",
            [FormResources.UsageCount] = x => x.UsageCount
        };

        [Inject] protected IPromoCodeService PromoCodeService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<PromoCodeDto>>> GetAllItemsAsync()
        {
            var result = await PromoCodeService.GetAllAsync();
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
            return await PromoCodeService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(PromoCodeDto item)
        {
            var result = await PromoCodeService.GetByIdAsync(item.Id);
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