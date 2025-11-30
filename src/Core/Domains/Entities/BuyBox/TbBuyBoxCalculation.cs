using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BuyBox
{
    public class TbBuyBoxCalculation : BaseEntity
    {
        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        [ForeignKey("WinningOffer")]
        public Guid WinningOfferId { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PriceScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal SellerRatingScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ShippingSpeedScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal FBMUsageScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal StockLevelScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ReturnRateScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalScore { get; set; }

        public DateTime CalculatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        [StringLength(1000)]
        public string? CalculationDetails { get; set; }

        public virtual TbItem Item { get; set; } = null!;
        public virtual TbOffer WinningOffer { get; set; } = null!;
    }
}
