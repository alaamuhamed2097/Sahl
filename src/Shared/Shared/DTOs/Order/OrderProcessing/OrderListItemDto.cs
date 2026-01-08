using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.OrderProcessing
{
    public class OrderListItemDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string SellerName { get; set; }
        public string ItemImageUrl { get; set; } = null!;
        public string ItemNameAr { get; set; }
        public string ItemNameEn { get; set; }
        public int QuantityItem { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}