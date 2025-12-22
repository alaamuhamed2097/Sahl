using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;
using Shared.DTOs.Base;
using Shared.DTOs.Order.Shipment;

namespace Shared.DTOs.Order.Order
{
    /// <summary>
    /// Stage 4: Shipment after splitting
    /// </summary>
    public class OrderShipmentDto : BaseDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public FulfillmentType FulfillmentType { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public List<ShipmentItemDto> Items { get; set; } = new();
        public List<ShipmentStatusHistoryDto> StatusHistory { get; set; } = new();
    }
}
