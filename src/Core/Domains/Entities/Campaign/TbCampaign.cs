using Common.Enumerations.Campaign;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
    public class TbCampaign : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        [Required]
        public CampaignType CampaignType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MinimumDiscountPercentage { get; set; } = 20m;

        [Required]
        public CampaignFundingModel FundingModel { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? PlatformFundingPercentage { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? SellerFundingPercentage { get; set; }

        [StringLength(500)]
        public string? BannerImagePath { get; set; }

        [StringLength(500)]
        public string? ThumbnailImagePath { get; set; }

        [StringLength(7)]
        public string? ThemeColor { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(100)]
        public string? SlugEn { get; set; }

        [StringLength(100)]
        public string? SlugAr { get; set; }

        public int? MaxParticipatingProducts { get; set; }

        public int? MaxProductsPerVendor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumOrderValue { get; set; }

        // New properties used by business logic
        public CampaignStatus Status { get; set; } = CampaignStatus.Draft;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BudgetLimit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSpent { get; set; } = 0m;

        public virtual ICollection<TbCampaignProduct> CampaignProducts { get; set; } = new HashSet<TbCampaignProduct>();
        public virtual ICollection<TbCampaignVendor> CampaignVendors { get; set; } = new HashSet<TbCampaignVendor>();
    }
}
