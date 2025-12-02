using Domains.Entities.Warehouse;
using Domains.Entities.Order;
using Domains.Entities.Shipping;

namespace Domains.Entities.Shipping
{
    public class TbOrderShipment : BaseEntity
    {
        public string ShipmentNumber { get; set; } = null!;
        public Guid OrderId { get; set; }
        public Guid VendorId { get; set; }
        public Guid? WarehouseId { get; set; }
        public int FulfillmentType { get; set; }
        public Guid? ShippingCompanyId { get; set; }
        public int ShipmentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        public virtual TbOrder Order { get; set; } = null!;
    }
}
