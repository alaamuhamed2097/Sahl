namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Shipment item
    /// </summary>
    public class ShipmentItemDto
    {
        public Guid ShipmentItemId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
