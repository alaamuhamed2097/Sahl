using Domains.Entities.Merchandising.HomePageBlocks;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Campaign
{
    public class TbCampaign : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string NameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string NameAr { get; set; } = string.Empty;

        // Timing
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Flash Sale
        public bool IsFlashSale { get; set; } = false;
        public DateTime? FlashSaleEndTime { get; set; }
        public int? MaxQuantityPerUser { get; set; }

        // Visual
        [StringLength(100)]
        public string? BadgeTextEn { get; set; }

        [StringLength(100)]
        public string? BadgeTextAr { get; set; }

        [StringLength(50)]
        public string? BadgeColor { get; set; }

        // Relations
        public ICollection<TbHomepageBlock> HomepageBlocks { get; set; }
        public ICollection<TbCampaignItem> CampaignItems { get; set; }

        public ICollection<TbCampaignVendor> CampaignVendors { get; set; }
    }
}
