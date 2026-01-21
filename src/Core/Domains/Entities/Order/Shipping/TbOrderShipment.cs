using Common.Enumerations.Fulfillment;
using Common.Enumerations.Shipping;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Shipping
{
    /// <summary>
    /// Shipment entity with complete pricing breakdown for COD support
    /// Each shipment can be paid separately (important for COD)
    /// FINAL VERSION
    /// </summary>
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
        public Guid WarehouseId { get; set; }

        // Fulfillment type enum (Marketplace or Vendor)
        public FulfillmentType FulfillmentType { get; set; }

        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompanyId { get; set; }

        // Shipment status enum
        public ShipmentStatus ShipmentStatus { get; set; }

        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        // ==================== PRICING BREAKDOWN ====================

        // Items subtotal (sum of all items in this shipment)
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        // Shipping cost for this specific shipment
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        // Tax amount allocated to this shipment
        // Formula: (SubTotal / Order.SubTotal) * Order.TaxAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        // Tax percentage (copied from order for reference)
        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxPercentage { get; set; }

        // Discount amount allocated to this shipment
        // Formula: (SubTotal / Order.SubTotal) * Order.DiscountAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        // Final total for this shipment
        // Formula: SubTotal + ShippingCost + TaxAmount - DiscountAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Weight information for shipping calculation
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalWeight { get; set; }

        // ==================== ADDITIONAL INFO ====================

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbWarehouse? Warehouse { get; set; }
        public virtual TbShippingCompany? ShippingCompany { get; set; }
        public virtual ICollection<TbOrderShipmentItem> Items { get; set; } = new HashSet<TbOrderShipmentItem>();
        public virtual ICollection<TbShipmentStatusHistory> StatusHistory { get; set; } = new HashSet<TbShipmentStatusHistory>();
        public virtual ICollection<TbShipmentPayment> ShipmentPayments { get; set; } = new HashSet<TbShipmentPayment>();
    }
}