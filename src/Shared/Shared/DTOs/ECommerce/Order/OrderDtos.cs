namespace Shared.DTOs.ECommerce.Order
{
    public class CreateOrderRequest
    {
        public Guid DeliveryAddressId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Guid? CouponId { get; set; }
    }

    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public Guid? ItemCombinationId { get; set; }
        public string? CombinationName { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class OrderSummaryDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public decimal Total { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public int ShipmentCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class OrderListItemDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = null!;
        public decimal Total { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
