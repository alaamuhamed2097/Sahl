using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
    public class TbFlashSaleProduct : BaseEntity
    {
        [Required]
        [ForeignKey("FlashSale")]
        public Guid FlashSaleId { get; set; }

        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FlashSalePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        public int SoldQuantity { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public int ViewCount { get; set; } = 0;

        public int AddToCartCount { get; set; } = 0;

        public virtual TbFlashSale FlashSale { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
    }
}
