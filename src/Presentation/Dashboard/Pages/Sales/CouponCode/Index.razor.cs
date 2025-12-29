using Dashboard.Constants;
using Dashboard.Contracts;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels;

namespace Dashboard.Pages.Sales.CouponCode
{
    public partial class Index : BaseListPage<CouponCodeDto>
    {
        protected override string EntityName { get; } = FormResources.PromoCodes;
        protected override string AddRoute { get; } = $"/couponCodee";
        protected override string EditRouteTemplate { get; } = "/couponCode/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.CouponCode.Search;
        protected override Dictionary<string, Func<CouponCodeDto, object>> ExportColumns { get; }
        = new Dictionary<string, Func<CouponCodeDto, object>>
        {
            [FormResources.ArabicTitle] = x => x.TitleAr,
            [FormResources.EnglishTitle] = x => x.TitleEn,
            [FormResources.Code] = x => x.Code,
            [FormResources.StartDate] = x => x.StartDate.ToString("yyyy-MM-dd"),
            [FormResources.EndDate] = x => x.ExpiryDate?.ToString("yyyy-MM-dd"),
            [FormResources.Type] = x => x.DiscountType,
            [FormResources.Value] = x => x.DiscountValue,
            [FormResources.UsageLimit] = x => x.UsageLimit?.ToString() ?? "∞",
            [FormResources.UsageCount] = x => x.UsageCount
        };

        [Inject] protected ICouponCodeService CouponCodeService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<CouponCodeDto>>> GetAllItemsAsync()
        {
            var result = await CouponCodeService.GetAllAsync();
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
            return await CouponCodeService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(CouponCodeDto item)
        {
            var result = await CouponCodeService.GetByIdAsync(item.Id);
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