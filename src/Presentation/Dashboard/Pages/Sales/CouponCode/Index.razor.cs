using Common.Enumerations.Order;
using Dashboard.Constants;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels;

namespace Dashboard.Pages.Sales.CouponCode
{
    public partial class Index : BaseListPage<CouponCodeDto>
    {
        protected override string EntityName { get; } = CouponCodeResources.PromoCodes;
        protected override string AddRoute { get; } = "/couponcode";
        protected override string EditRouteTemplate { get; } = "/couponcode/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.CouponCode.Search;

        [Inject] protected ICouponCodeService CouponCodeService { get; set; } = null!;
        [Inject] protected IDateTimeService DateTimeService { get; set; } = null!;

        protected override Dictionary<string, Func<CouponCodeDto, object>> ExportColumns { get; }
            = new()
            {
                [CouponCodeResources.Code] = x => x.Code,
                [CouponCodeResources.ArabicTitle] = x => x.TitleAr,
                [CouponCodeResources.EnglishTitle] = x => x.TitleEn,
                [CouponCodeResources.Type] = x => GetPromoTypeDisplayForExport(x.PromoType),
                [CouponCodeResources.DiscountType] = x =>
                    x.DiscountType == DiscountType.Percentage
                        ? CouponCodeResources.PercentageDiscount
                        : CouponCodeResources.FixedAmountDiscount,
                [CouponCodeResources.Value] = x => x.DiscountValue,
                [CouponCodeResources.StartDate] = x => x.StartDate.ToString("yyyy-MM-dd") ?? "-",
                [CouponCodeResources.EndDate] = x => x.ExpiryDate?.ToString("yyyy-MM-dd") ?? "-",
                [CouponCodeResources.MinimumOrderAmount] = x => x.MinimumOrderAmount?.ToString() ?? "-",
                [CouponCodeResources.MaxDiscountAmount] = x => x.MaxDiscountAmount?.ToString() ?? "-",
                [CouponCodeResources.UsageLimit] = x => x.UsageLimit?.ToString() ?? CouponCodeResources.UnlimitedUsage,
                [CouponCodeResources.UsageCount] = x => x.UsageCount,
                [CouponCodeResources.UsageLimitPerUser] = x => x.UsageLimitPerUser?.ToString() ?? "-",
                [CouponCodeResources.Status] = x => x.IsActive
                    ? GeneralResources.Active
                    : GeneralResources.Inactive,
                [CouponCodeResources.IsFirstOrderOnly] = x => x.IsFirstOrderOnly
                    ? GeneralResources.Yes
                    : GeneralResources.No
            };

        protected override async Task<ResponseModel<IEnumerable<CouponCodeDto>>> GetAllItemsAsync()
        {
            try
            {
                return await CouponCodeService.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllItemsAsync: {ex.Message}");
                return new ResponseModel<IEnumerable<CouponCodeDto>>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrongAlert
                };
            }
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            try
            {
                return await CouponCodeService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteItemAsync: {ex.Message}");
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrongAlert
                };
            }
        }

        protected override async Task<string> GetItemId(CouponCodeDto item)
        {
            try
            {
                return item?.Id != Guid.Empty ? item.Id.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetItemId: {ex.Message}");
                return string.Empty;
            }
        }

        private static string GetPromoTypeDisplayForExport(CouponCodeType type)
        {
            return type switch
            {
                CouponCodeType.General => CouponCodeResources.GeneralCoupon,
                CouponCodeType.CategoryBased => CouponCodeResources.CategoryBasedCoupon,
                CouponCodeType.VendorBased => CouponCodeResources.VendorBasedCoupon,
                CouponCodeType.NewUserOnly => CouponCodeResources.NewUserOnlyCoupon,
                _ => type.ToString()
            };
        }

        protected string GetPromoTypeDisplay(CouponCodeType type)
        {
            return type switch
            {
                CouponCodeType.General => CouponCodeResources.GeneralCoupon,
                CouponCodeType.CategoryBased => CouponCodeResources.CategoryBasedCoupon,
                CouponCodeType.VendorBased => CouponCodeResources.VendorBasedCoupon,
                CouponCodeType.NewUserOnly => CouponCodeResources.NewUserOnlyCoupon,
                _ => type.ToString()
            };
        }

        protected string GetPromoTypeBadgeClass(CouponCodeType type)
        {
            return type switch
            {
                CouponCodeType.General => "bg-primary",
                CouponCodeType.CategoryBased => "bg-info",
                CouponCodeType.VendorBased => "bg-success",
                CouponCodeType.NewUserOnly => "bg-warning text-dark",
                _ => "bg-secondary"
            };
        }
    }
}