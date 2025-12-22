using Common.Enumerations.Fulfillment;

namespace Shared.DTOs.Order
{
    /// <summary>
    /// Expected shipment preview during checkout
    /// </summary>
    public class ExpectedShipmentDto
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public FulfillmentType FulfillmentType { get; set; }
        public int ItemCount { get; set; }
        public List<string> ItemNames { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal EstimatedShippingCost { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
