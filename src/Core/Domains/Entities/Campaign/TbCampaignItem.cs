using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
    public class TbCampaignItem : BaseEntity
    {
        [Required]
        [ForeignKey("Campaign")]
        public Guid CampaignId { get; set; }

        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        // Campaign Price (الأهم!)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CampaignPrice { get; set; }

        // Flash Sale Stock Management
        public int? StockLimit { get; set; }  // الكمية المتاحة للعرض (null = unlimited)
        public int SoldCount { get; set; } = 0;  // كام اتباع

        // Display
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        // Relations
        public virtual TbCampaign Campaign { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
    }
}
