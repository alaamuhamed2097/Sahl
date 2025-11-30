using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Fulfillment
{
    public class TbFBMInventory : BaseEntity
    {
        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public int ReservedQuantity { get; set; }

        [Required]
        public int InTransitQuantity { get; set; }

        [Required]
        public int DamagedQuantity { get; set; }

        public DateTime? LastSyncDate { get; set; }

        [StringLength(100)]
        public string? SKU { get; set; }

        [StringLength(100)]
        public string? LocationCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual TbItem Item { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbWarehouse Warehouse { get; set; } = null!;
    }
}
