using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.OrderProcessing.CustomerOrder
{
    /// <summary>
    /// Customer Order List Item - للعرض في قائمة الطلبات
    /// </summary>
    public class CustomerOrderListDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderProgressStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }

        // Items Summary
        public int TotalItems { get; set; }
        public List<OrderItemSummaryDto> ItemsSummary { get; set; } = new();

        // Can Cancel/Refund
        public bool CanCancel { get; set; }
        public bool IsWithinRefundPeriod { get; set; }
    }
}
