namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Preview of how order will be split into shipments
    /// </summary>
    public class ShipmentPreviewDto
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public List<string> ItemsList { get; set; } = new();
        public List<CheckoutItemDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public int EstimatedDeliveryDays { get; set; }
    }
}