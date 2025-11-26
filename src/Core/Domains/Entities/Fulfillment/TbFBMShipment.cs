using Common.Enumerations.Fulfillment;
using Domains.Entities.Base;
using Domains.Entities.Order;
using Domains.Entities.Shipping;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Fulfillment
{
    public class TbFBMShipment : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string ShipmentNumber { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [Required]
        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }

        [Required]
        [ForeignKey("ShippingCompany")]
        public Guid ShippingCompanyId { get; set; }

        [Required]
        public FBMShipmentStatus Status { get; set; }

        [StringLength(100)]
        public string? TrackingNumber { get; set; }

        public DateTime? PickupDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? ActualWeight { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? VolumetricWeight { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [StringLength(1000)]
        public string? DeliveryNotes { get; set; }

        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbWarehouse Warehouse { get; set; } = null!;
        public virtual TbShippingCompany ShippingCompany { get; set; } = null!;
    }
}
