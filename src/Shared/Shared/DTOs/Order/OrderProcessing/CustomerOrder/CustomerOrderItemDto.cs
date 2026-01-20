using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.OrderProcessing.CustomerOrder
{
    public class CustomerOrderItemDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemImage { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
    }
}
