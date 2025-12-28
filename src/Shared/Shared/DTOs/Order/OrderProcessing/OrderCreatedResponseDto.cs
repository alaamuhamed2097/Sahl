using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Shared.DTOs.Base;

namespace Shared.DTOs.Order.OrderProcessing
{
    /// <summary>
    /// Stage 3: Order Created Response
    /// </summary>
    public class OrderCreatedResponseDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public OrderProgressStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int ShipmentCount { get; set; }
        public List<OrderDetailItemDto> OrderDetails { get; set; } = new();
        public string Message { get; set; } = null!;
    }
}
