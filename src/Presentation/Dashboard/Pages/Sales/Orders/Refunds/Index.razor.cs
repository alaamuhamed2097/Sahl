using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Resources;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.Payment.Refund;
using Shared.GeneralModels;

namespace Dashboard.Pages.Sales.Orders.Refunds
{
    public partial class Index : BaseListPage<RefundRequestDto>
    {
        protected override string EntityName { get; } = OrderResources.Refunds;
        protected override string AddRoute { get; } = $"/sales/refunds";
        protected override string EditRouteTemplate { get; } = "/sales/refunds/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Refund.Search;
        protected new RefundSearchCriteria searchModel { get; set; } = new();
        protected override Dictionary<string, Func<RefundRequestDto, object>> ExportColumns { get; } =
            new Dictionary<string, Func<RefundRequestDto, object>>
            {
                [OrderResources.RefundNumber] = x => x.Number,
                [OrderResources.RequestedDate] = x => x.RequestDateLocalFormatted,
                [OrderResources.ItemsCount] = x => x.ApprovedItemsCount,
                [OrderResources.RefundAmount] = x => $"{x.RefundAmount} $",
                [OrderResources.RefundStatus] = x => x.RefundStatus switch
                {
                    RefundStatus.Open => OrderResources.Opened,
                    RefundStatus.UnderReview => ECommerceResources.UnderReview,
                    RefundStatus.NeedMoreInfo => OrderResources.NeedMoreInfo,
                    RefundStatus.InfoApproved => OrderResources.InfoApproved,
                    RefundStatus.ItemShippedBack => OrderResources.ItemShippedBack,
                    RefundStatus.ItemReceived => OrderResources.ItemReceived,
                    RefundStatus.Inspecting => OrderResources.Inspecting,
                    RefundStatus.Approved => ECommerceResources.Approved,
                    RefundStatus.Rejected => ECommerceResources.Rejected,
                    RefundStatus.Refunded => OrderResources.Refunded,
                    RefundStatus.Closed => OrderResources.Closed,
                    _ => x.RefundStatus.ToString()
                }
            };

        [Inject] protected IRefundService RefundService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<RefundRequestDto>>> GetAllItemsAsync()
        {
            return await RefundService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await RefundService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(RefundRequestDto refund)
        {
            return refund.Id.ToString();
        }

        protected override async Task Search()
        {
            try
            {
                await OnBeforeSearchAsync();

                var result = await RefundService.SearchAsync(searchModel, SearchEndpoint);
                if (result.Success)
                {
                    items = result.Data?.Items ?? new List<RefundRequestDto>();
                    totalRecords = result.Data?.TotalRecords ?? 0;
                    totalPages = (int)Math.Ceiling((double)totalRecords / searchModel.PageSize);
                    currentPage = searchModel.PageNumber;
                    StateHasChanged();

                    await OnAfterSearchAsync();
                }
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("swal", ValidationResources.Error, NotifiAndAlertsResources.FailedToRetrieveData, "error");
            }
        }

        protected string GetRefundStatusClass(RefundStatus status)
        {
            return status switch
            {
                RefundStatus.Open => "badge bg-warning",
                RefundStatus.UnderReview => "badge bg-danger",
                RefundStatus.NeedMoreInfo => "badge bg-primary",
                RefundStatus.InfoApproved => "badge bg-info",
                RefundStatus.ItemShippedBack => "badge bg-success",
                RefundStatus.ItemReceived => "badge bg-secondary",
                RefundStatus.Inspecting => "badge bg-dark",
                RefundStatus.Approved => "badge bg-dark",
                RefundStatus.Rejected => "badge bg-dark",
                RefundStatus.Refunded => "badge bg-dark",
                RefundStatus.Closed => "badge bg-secondary",
                _ => "badge bg-secondary"
            };
        }

        protected string GetRefundStatusText(RefundStatus status)
        {
            return status switch
            {
                RefundStatus.Open => OrderResources.Opened,
                RefundStatus.UnderReview => ECommerceResources.UnderReview,
                RefundStatus.NeedMoreInfo => OrderResources.NeedMoreInfo,
                RefundStatus.InfoApproved => OrderResources.InfoApproved,
                RefundStatus.ItemShippedBack => OrderResources.ItemShippedBack,
                RefundStatus.ItemReceived => OrderResources.ItemReceived,
                RefundStatus.Inspecting => OrderResources.Inspecting,
                RefundStatus.Approved => ECommerceResources.Approved,
                RefundStatus.Rejected => ECommerceResources.Rejected,
                RefundStatus.Refunded => OrderResources.Refunded,
                RefundStatus.Closed => OrderResources.Closed,
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Handle Refund Status filter change
        /// </summary>
        protected async Task OnRefundStatusFilterChanged(ChangeEventArgs e)
        {
            if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
            {
                if (int.TryParse(e.Value.ToString(), out int statusValue))
                {
                    searchModel.Status = statusValue == 0 ? null : (RefundStatus) statusValue;
                    currentPage = 1;
                    searchModel.PageNumber = 1;
                    await Search();
                }
            }
        }
    }
}

