namespace Shared.DTOs.ECommerce.Shipment
{
    public class ShipmentDto
    {
        public Guid Id { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public Guid OrderId { get; set; }
        public string VendorName { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;
        public int FulfillmentType { get; set; }
        public string ShipmentStatus { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ShipmentItemDto> Items { get; set; } = new();
        public List<ShipmentStatusHistoryDto> StatusHistory { get; set; } = new();
    }

    public class ShipmentItemDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public Guid? ItemCombinationId { get; set; }
        public string? CombinationName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class ShipmentStatusHistoryDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime StatusDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateShipmentRequest
    {
        public Guid OrderId { get; set; }
    }

    public class UpdateShipmentStatusRequest
    {
        public Guid ShipmentId { get; set; }
        public string NewStatus { get; set; } = null!;
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }

    public class AssignTrackingNumberRequest
    {
        public Guid ShipmentId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public DateTime? EstimatedDeliveryDate { get; set; }
    }

    public class ShipmentTrackingDto
    {
        public string ShipmentNumber { get; set; } = null!;
        public string TrackingNumber { get; set; } = null!;
        public string CurrentStatus { get; set; } = null!;
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public string VendorName { get; set; } = null!;
        public List<ShipmentStatusHistoryDto> History { get; set; } = new();
    }
}
