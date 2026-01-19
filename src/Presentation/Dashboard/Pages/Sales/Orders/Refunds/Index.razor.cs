using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Dashboard.Constants;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.Payment.Refund;
using Shared.GeneralModels;

namespace Dashboard.Pages.Sales.Orders.Refunds
{
    public partial class Index : BaseListPage<RefundRequestDto>
    {
        private static int iterator = 0;
        protected override string EntityName { get; } = ECommerceResources.Refunds;
        protected override string AddRoute { get; } = $"/sales/refunds";
        protected override string EditRouteTemplate { get; } = "/sales/refunds/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Refund.Search;
        protected override Dictionary<string, Func<RefundRequestDto, object>> ExportColumns { get; } =
            new Dictionary<string, Func<RefundRequestDto, object>>
            {
                [ECommerceResources.RefundNumber] = x => x.Number,
                [ECommerceResources.RequestedDate] = x => x.RequestDateLocalFormatted,
                [ECommerceResources.ApprovedItemsCount] = x => x.ApprovedItemsCount,
                [FormResources.RefundAmount] = x => $"{x.RefundAmount} $",
                [ECommerceResources.RefundStatus] = x => x.RefundStatus switch
                {
                    RefundStatus.Open => ECommerceResources.Opened,
                    RefundStatus.UnderReview => ECommerceResources.UnderReview,
                    RefundStatus.NeedMoreInfo => ECommerceResources.NeedMoreInfo,
                    RefundStatus.InfoApproved => ECommerceResources.InfoApproved,
                    RefundStatus.ItemShippedBack => ECommerceResources.ItemShippedBack,
                    RefundStatus.ItemReceived => ECommerceResources.ItemReceived,
                    RefundStatus.Inspecting => ECommerceResources.Inspecting,
                    RefundStatus.Approved => ECommerceResources.Approved,
                    RefundStatus.Rejected => ECommerceResources.Rejected,
                    RefundStatus.Refunded => ECommerceResources.Refunded,
                    RefundStatus.Closed => ECommerceResources.Closed,
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
                RefundStatus.Open => ECommerceResources.Opened,
                RefundStatus.UnderReview => ECommerceResources.UnderReview,
                RefundStatus.NeedMoreInfo => ECommerceResources.NeedMoreInfo,
                RefundStatus.InfoApproved => ECommerceResources.InfoApproved,
                RefundStatus.ItemShippedBack => ECommerceResources.ItemShippedBack,
                RefundStatus.ItemReceived => ECommerceResources.ItemReceived,
                RefundStatus.Inspecting => ECommerceResources.Inspecting,
                RefundStatus.Approved => ECommerceResources.Approved,
                RefundStatus.Rejected => ECommerceResources.Rejected,
                RefundStatus.Refunded => ECommerceResources.Refunded,
                RefundStatus.Closed => ECommerceResources.Closed,
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
                    searchModel.SearchTerm = $"status:{statusValue}";
                }
            }
            else
            {
                // Clear the status filter if empty option selected
                searchModel.SearchTerm = "";
            }

            currentPage = 1;
            searchModel.PageNumber = 1;
            await Search();
        }

        /// <summary>
        /// Handle Payment Status filter change
        /// </summary>
        protected async Task OnPaymentStatusFilterChanged(ChangeEventArgs e)
        {
            if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
            {
                if (int.TryParse(e.Value.ToString(), out int paymentStatusValue))
                {
                    searchModel.SearchTerm = $"payment:{paymentStatusValue}";
                }
            }
            else
            {
                // Clear the payment status filter if empty option selected
                searchModel.SearchTerm = "";
            }

            currentPage = 1;
            searchModel.PageNumber = 1;
            await Search();
        }
    }
}

