using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Dashboard.Constants;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;
using Shared.GeneralModels;

namespace Dashboard.Pages.Orders.Orders
{
    /// <summary>
    /// Index page for Orders list - CLEAN VERSION
    /// Inherits from BaseListPage for consistent UI/UX
    /// Works directly with API DTOs - NO intermediate mapping
    /// </summary>
    public partial class Index : BaseListPage<AdminOrderListDto>
    {
        // ============================================
        // REQUIRED OVERRIDES FROM BaseListPage
        // ============================================

        protected override string EntityName { get; } = ECommerceResources.Orders;
        protected override string AddRoute { get; } = "/order/orders/create";
        protected override string EditRouteTemplate { get; } = "/order/orders/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Order.Search;

        /// <summary>
        /// Export columns definition - uses API DTO properties directly
        /// </summary>
        protected override Dictionary<string, Func<AdminOrderListDto, object>> ExportColumns { get; } =
            new Dictionary<string, Func<AdminOrderListDto, object>>
            {
                [OrderResources.OrderID] = x => x.OrderNumber,
                [OrderResources.CustomerName] = x => x.CustomerName,
                [FormResources.PhoneNumber] = x => $"{x.CustomerPhoneCode} {x.CustomerPhone}",
                [OrderResources.OrderDate] = x => x.OrderDate.ToString("dd/MM/yyyy hh:mm tt"),
                [OrderResources.OrderStatus] = x => x.OrderStatus switch
                {
                    OrderProgressStatus.Pending => OrderResources.Pending,
                    OrderProgressStatus.Confirmed => OrderResources.Accepted,
                    OrderProgressStatus.Processing => OrderResources.InProgress,
                    OrderProgressStatus.Shipped => OrderResources.Shipping,
                    OrderProgressStatus.Delivered => OrderResources.Delivered,
                    OrderProgressStatus.Completed => OrderResources.Completed,
                    OrderProgressStatus.Cancelled => OrderResources.Canceled,
                    OrderProgressStatus.PaymentFailed => OrderResources.PaymentFailed,
                    OrderProgressStatus.RefundRequested => OrderResources.RefundRequested,
                    OrderProgressStatus.Refunded => OrderResources.Refunded,
                    OrderProgressStatus.Returned => OrderResources.Returned,
                    _ => x.OrderStatus.ToString()
                },
                [ECommerceResources.PaymentStatus] = x => x.PaymentStatus switch
                {
                    PaymentStatus.Completed => ECommerceResources.Paid,
                    PaymentStatus.Pending => ECommerceResources.Pending,
                    PaymentStatus.Processing => OrderResources.Processing,
                    PaymentStatus.Failed => OrderResources.Failed,
                    _ => x.PaymentStatus.ToString()
                },
                [OrderResources.ItemsCount] = x => x.TotalItemsCount,
                [FormResources.Price] = x => $"{x.TotalAmount:F2} $"
            };

        // ============================================
        // DEPENDENCY INJECTION
        // ============================================

        [Inject] protected IOrderService OrderService { get; set; } = null!;

        // ============================================
        // CRUD OPERATIONS - Required by BaseListPage
        // ============================================

        /// <summary>
        /// Get all orders - used for initial load (legacy support)
        /// BaseListPage will use Search() for actual pagination
        /// </summary>
        protected override async Task<ResponseModel<IEnumerable<AdminOrderListDto>>> GetAllItemsAsync()
        {
            // This is called by BaseListPage, but we use Search instead
            // Return empty for now - Search handles everything
            return await Task.FromResult(new ResponseModel<IEnumerable<AdminOrderListDto>>
            {
                Success = true,
                Data = new List<AdminOrderListDto>()
            });
        }

        /// <summary>
        /// Delete/Cancel order
        /// </summary>
        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await OrderService.CancelOrderAsync(id, "Cancelled from dashboard");
        }

        /// <summary>
        /// Get item ID for navigation - uses API DTO property
        /// </summary>
        protected override async Task<string> GetItemId(AdminOrderListDto item)
        {
            return await Task.FromResult(item.OrderId.ToString());
        }

        // ============================================
        // UI HELPERS - Work with API DTOs directly
        // ============================================

        /// <summary>
        /// Get CSS class for order status badge
        /// Works with API DTO OrderStatus property
        /// </summary>
        protected string GetOrderStatusClass(OrderProgressStatus status)
        {
            return status switch
            {
                OrderProgressStatus.Pending => "badge bg-secondary",
                OrderProgressStatus.Confirmed => "badge bg-warning",
                OrderProgressStatus.Processing => "badge bg-primary",
                OrderProgressStatus.Shipped => "badge bg-info",
                OrderProgressStatus.Delivered => "badge bg-success",
                OrderProgressStatus.Completed => "badge bg-success",
                OrderProgressStatus.Cancelled => "badge bg-dark",
                OrderProgressStatus.PaymentFailed => "badge bg-danger",
                OrderProgressStatus.RefundRequested => "badge bg-warning",
                OrderProgressStatus.Refunded => "badge bg-secondary",
                OrderProgressStatus.Returned => "badge bg-secondary",
                _ => "badge bg-secondary"
            };
        }

        /// <summary>
        /// Get localized text for order status
        /// </summary>
        protected string GetOrderStatusText(OrderProgressStatus status)
        {
            return status switch
            {
                OrderProgressStatus.Pending => OrderResources.Pending,
                OrderProgressStatus.Confirmed => OrderResources.Accepted,
                OrderProgressStatus.Processing => OrderResources.InProgress,
                OrderProgressStatus.Shipped => OrderResources.Shipping,
                OrderProgressStatus.Delivered => OrderResources.Delivered,
                OrderProgressStatus.Completed => OrderResources.Completed,
                OrderProgressStatus.Cancelled => OrderResources.Canceled,
                OrderProgressStatus.PaymentFailed => OrderResources.PaymentFailed,
                OrderProgressStatus.RefundRequested => OrderResources.RefundRequested,
                OrderProgressStatus.Refunded => OrderResources.Refunded,
                OrderProgressStatus.Returned => OrderResources.Returned,
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Get CSS class for payment status
        /// </summary>
        protected string GetPaymentStatusClass(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Completed => "text-success",
                PaymentStatus.Pending => "text-primary",
                PaymentStatus.Processing => "text-info",
                PaymentStatus.Failed => "text-danger",
                PaymentStatus.Cancelled => "text-secondary",
                PaymentStatus.Refunded => "text-info",
                _ => "text-secondary"
            };
        }

        /// <summary>
        /// Get localized text for payment status
        /// </summary>
        protected string GetPaymentStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Completed => ECommerceResources.Paid,
                PaymentStatus.Pending => ECommerceResources.Pending,
                PaymentStatus.Processing => OrderResources.Processing,
                PaymentStatus.Failed => OrderResources.Failed,
                PaymentStatus.Cancelled => OrderResources.Cancelled,
                PaymentStatus.Refunded => OrderResources.Refunded,
                _ => status.ToString()
            };
        }

        // ============================================
        // FILTER HANDLERS
        // ============================================

        /// <summary>
        /// Handle Order Status filter change
        /// Updates search term with status filter format
        /// </summary>
        protected async Task OnOrderStatusFilterChanged(ChangeEventArgs e)
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
                searchModel.SearchTerm = "";
            }

            currentPage = 1;
            searchModel.PageNumber = 1;
            await Search();
        }

        // ============================================
        // CUSTOM INITIALIZATION
        // ============================================

        /// <summary>
        /// Custom initialization logic
        /// Called by BaseListPage before initial search
        /// </summary>
        protected override async Task OnCustomInitializeAsync()
        {
            // Set default sort to OrderDate descending
            searchModel.SortBy = "OrderDate";
            searchModel.SortDirection = "desc";

            await base.OnCustomInitializeAsync();
        }

        /// <summary>
        /// Logic to execute after each search
        /// </summary>
        protected override async Task OnAfterSearchAsync()
        {
            // Any post-search logic here
            await base.OnAfterSearchAsync();
        }
    }
}