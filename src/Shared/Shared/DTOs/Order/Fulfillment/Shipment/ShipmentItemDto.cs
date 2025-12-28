namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    /// <summary>
    /// Shipment item DTO
    /// </summary>
    public class ShipmentItemDto
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemImage { get; set; }
        public Guid? ItemCombinationId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
