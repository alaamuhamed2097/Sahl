using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.Payment;

namespace Shared.DTOs.Order.OrderProcessing.CustomerOrder
{
    /// <summary>
    /// Customer Order Details - تفاصيل كاملة للطلب
    /// </summary>
    public class CustomerOrderDetailsDto
    {
        // Order Info
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public OrderProgressStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        // Financial Info
        public decimal SubTotal { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Delivery Address
        public DeliveryAddressDto DeliveryAddress { get; set; } = null!;

        // Payment Info
        public PaymentInfoDto PaymentInfo { get; set; } = null!;

        // Items
        public List<CustomerOrderItemDto> Items { get; set; } = new();

        // Shipment Info
        public List<ShipmentInfoDto> Shipments { get; set; } = new();

        // Actions
        public bool CanCancel { get; set; }
        public bool CanRequestRefund { get; set; }
        public bool IsWithinRefundPeriod { get; set; }
    }
}
