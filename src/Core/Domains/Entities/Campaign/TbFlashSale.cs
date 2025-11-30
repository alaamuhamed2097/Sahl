using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
    public class TbFlashSale : BaseEntity
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
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(6, 48)]
        public int DurationInHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MinimumDiscountPercentage { get; set; } = 20m;

        [Column(TypeName = "decimal(3,2)")]
        public decimal MinimumSellerRating { get; set; } = 4.0m;

        [StringLength(500)]
        public string? BannerImagePath { get; set; }

        [StringLength(7)]
        public string? ThemeColor { get; set; }

        public bool IsActive { get; set; } = true;

        public bool ShowCountdownTimer { get; set; } = true;

        public int DisplayOrder { get; set; }

        public int? MaxProducts { get; set; }

        public int TotalSales { get; set; } = 0;

        public virtual ICollection<TbFlashSaleProduct> FlashSaleProducts { get; set; } = new HashSet<TbFlashSaleProduct>();
    }
}
