using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Dashboard.Constants;
using Dashboard.Contracts.Order;
using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.Order.OrderProcessing;
using Shared.GeneralModels;

namespace Dashboard.Pages.Sales.Orders
{
    public partial class Index : BaseListPage<OrderDto>
    {
        private static int iterator = 0;
        protected override string EntityName { get; } = ECommerceResources.Orders;
        protected override string AddRoute { get; } = $"/sales/orders/{Guid.Empty}";
        protected override string EditRouteTemplate { get; } = "/sales/orders/{id}";
        protected override string SearchEndpoint { get; } = ApiEndpoints.Order.Search;
        protected override Dictionary<string, Func<OrderDto, object>> ExportColumns { get; } =
            new Dictionary<string, Func<OrderDto, object>>
            {
                [ECommerceResources.OrderID] = x => ++(iterator),
                [ECommerceResources.CustomerName] = x => $"{x.FirstName} {x.LastName}",
                [ECommerceResources.OrderDate] = x => x.CreatedDateLocalFormatted,
                [ECommerceResources.OrderStatus] = x => x.CurrentState switch
                {
                    OrderStatus.Accepted => ECommerceResources.Accepted,
                    OrderStatus.Rejected => ECommerceResources.Rejected,
                    OrderStatus.InProgress => ECommerceResources.InProgress,
                    OrderStatus.Shipping => ECommerceResources.Shipping,
                    OrderStatus.Delivered => ECommerceResources.Delivered,
                    OrderStatus.Pending => ECommerceResources.Pending,
                    OrderStatus.Canceled => ECommerceResources.Canceled,
                    OrderStatus.Returned => ECommerceResources.Returned,
                    _ => x.CurrentState.ToString()
                },
                [ECommerceResources.PaymentStatus] = x => x.PaymentStatus switch
                {
                    PaymentStatus.Completed => ECommerceResources.Paid,
                    PaymentStatus.Pending => ECommerceResources.Pending,
                    _ => x.PaymentStatus.ToString()
                },
                [FormResources.Price] = x => $"{x.Price} $"
            };

        [Inject] protected IOrderService OrderService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<OrderDto>>> GetAllItemsAsync()
        {
            return await OrderService.GetAllAsync();
        }

        protected override async Task<ResponseModel<bool>> DeleteItemAsync(Guid id)
        {
            return await OrderService.DeleteAsync(id);
        }

        protected override async Task<string> GetItemId(OrderDto item)
        {
            return item.Id.ToString();
        }

        protected string GetOrderStatusClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Accepted => "badge bg-warning",
                OrderStatus.Rejected => "badge bg-danger",
                OrderStatus.InProgress => "badge bg-primary",
                OrderStatus.Shipping => "badge bg-info",
                OrderStatus.Delivered => "badge bg-success",
                OrderStatus.Pending => "badge bg-secondary",
                OrderStatus.Canceled => "badge bg-dark",
                OrderStatus.Returned => "badge bg-secondary",
                _ => "badge bg-secondary"
            };
        }

        protected string GetOrderStatusText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Accepted => ECommerceResources.Accepted,
                OrderStatus.Rejected => ECommerceResources.Rejected,
                OrderStatus.InProgress => ECommerceResources.InProgress,
                OrderStatus.Shipping => ECommerceResources.Shipping,
                OrderStatus.Delivered => ECommerceResources.Delivered,
                OrderStatus.Pending => ECommerceResources.Pending,
                OrderStatus.Canceled => ECommerceResources.Canceled,
                OrderStatus.Returned => ECommerceResources.Returned,
                _ => status.ToString()
            };
        }

        protected string GetPaymentStatusClass(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Completed => "text-success",
                PaymentStatus.Pending => "text-primary",
                _ => "text-secondary"
            };
        }

        protected string GetPaymentStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Completed => ECommerceResources.Paid,
                PaymentStatus.Pending => ECommerceResources.Pending,
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Handle Order Status filter change
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

