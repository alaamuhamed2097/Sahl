using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Shipping
{
    public class TbOrderShipment : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Number { get; set; } = null!;

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

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbWarehouse? Warehouse { get; set; }
        public virtual TbShippingCompany? ShippingCompany { get; set; }
        public virtual ICollection<TbOrderShipmentItem> Items { get; set; } = new HashSet<TbOrderShipmentItem>();
        public virtual ICollection<TbShipmentStatusHistory> StatusHistory { get; set; } = new HashSet<TbShipmentStatusHistory>();
    }
}
