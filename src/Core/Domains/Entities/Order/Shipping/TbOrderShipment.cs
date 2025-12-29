using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Shipping
{
    public class TbOrderShipment : BaseEntity
    {
        public string ShipmentNumber { get; set; } = null!;

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; } // The warehouse where it will be shipped

        // Use enum for fulfillment type 
        public FulfillmentType FulfillmentType { get; set; }

        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompanyId { get; set; }

        // Use enum for shipment status
        public ShipmentStatus ShipmentStatus { get; set; }

        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbWarehouse? Warehouse { get; set; }
        public virtual TbShippingCompany? ShippingCompany { get; set; }

        // Add relation to shipment items
        public virtual ICollection<TbOrderShipmentItem> Items { get; set; } = new HashSet<TbOrderShipmentItem>();
    }
}
