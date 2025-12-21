using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BuyBox
{
    public class TbBuyBoxCalculation : BaseEntity
    {
        [Required]
        public Guid ItemId { get; set; }

        /// <summary>
        /// CRITICAL: BuyBox is calculated per combination, not just item
        /// </summary>
        [Required]
        public Guid ItemCombinationId { get; set; }

        /// <summary>
        /// The winning offer for this specific combination
        /// </summary>
        [Required]
        public Guid WinningOfferCombinationId { get; set; }

        /// <summary>
        /// Backwards-compatible individual score components (0-100 scaled using decimal(5,2))
        /// These fields are kept for existing configuration & indexes.
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PriceScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal SellerRatingScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ShippingSpeedScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal FBMUsageScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal StockLevelScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ReturnRateScore { get; set; }

        /// <summary>
        /// Backwards-compatible total score (0-100)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalScore { get; set; }

        /// <summary>
        /// New unified score with higher precision
        /// </summary>
        [Column(TypeName = "decimal(18,4)")]
        public decimal Score { get; set; }

        /// <summary>
        /// When this calculation was performed
        /// </summary>
        public DateTime CalculatedAt { get; set; }

        /// <summary>
        /// How long this BuyBox is valid (cache duration)
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        [StringLength(1000)]
        public string? CalculationDetails { get; set; }

        /// <summary>
        /// Breakdown of score components for transparency (JSON)
        /// </summary>
        public string? ScoreBreakdown { get; set; }

        // Navigation Properties
        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;

        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; } = null!;

        [ForeignKey("WinningOfferCombinationId")]
        public virtual TbOfferCombinationPricing WinningOfferCombination { get; set; } = null!;
    }
}
