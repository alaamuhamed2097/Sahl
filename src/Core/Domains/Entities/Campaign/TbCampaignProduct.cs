using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
    public class TbCampaignProduct : BaseEntity
    {
        [Required]
        [ForeignKey("Campaign")]
        public Guid CampaignId { get; set; }

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
        public decimal CampaignPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PlatformContribution { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? VendorContribution { get; set; }

        // New property to track available stock in campaign
        public int? StockQuantity { get; set; }

        public int SoldQuantity { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("ApprovedByUser")]
        public string? ApprovedByUserId { get; set; }

        public int DisplayOrder { get; set; }

        public virtual TbCampaign Campaign { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual ApplicationUser? ApprovedByUser { get; set; }
    }
}
