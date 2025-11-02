using Microsoft.AspNetCore.Components;
using Resources;
using Shared.DTOs.ECommerce;
using Common.Enumerations.Payment;
using Shared.GeneralModels;
using Dashboard.Constants;
using Dashboard.Contracts.Order;
using Common.Enumerations.Order;

namespace Dashboard.Pages.Sales.Orders
{
    public partial class Index : BaseListPage<OrderDto>
    {
        private static int iterator = 0;
        protected override string EntityName { get; } = ECommerceResources.Orders;
        protected override string AddRoute { get; } = "/order";
        protected override string EditRouteTemplate { get; } = $"/order/{{id}}";
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
             PaymentStatus.Paid => ECommerceResources.Paid,
             PaymentStatus.Pending => ECommerceResources.Pending,
             // PaymentStatus.Failed => ECommerceResources.Failed,
             _ => x.PaymentStatus.ToString()
         },
         [FormResources.Price] = x => $"{x.Price} $"
     };

        [Inject] protected IOrderService OrderService { get; set; } = null!;

        protected override async Task<ResponseModel<IEnumerable<OrderDto>>> GetAllItemsAsync()
        {
            var result = await OrderService.GetAllAsync();
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
            return await OrderService.DeleteAsync(id);
        }
        protected override async Task<string> GetItemId(OrderDto item)
        {
            return item.Id.ToString();
        }
    }
}
