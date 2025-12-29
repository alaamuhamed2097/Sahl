using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    /// <summary>
    /// SINGLE CONSOLIDATED ShipmentDto - Use this everywhere
    /// DELETE any other ShipmentDto definitions
    /// </summary>
    public class ShipmentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string ShipmentNumber { get; set; } = string.Empty;
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public Guid? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public FulfillmentType FulfillmentType { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public Guid? ShippingCompanyId { get; set; }
        public string? ShippingCompanyName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public List<ShipmentItemDto> Items { get; set; } = new();
    }
}