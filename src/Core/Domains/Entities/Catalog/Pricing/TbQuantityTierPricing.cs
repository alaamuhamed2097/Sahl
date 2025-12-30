using Domains.Entities.Offer;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Pricing
{
    public class TbQuantityTierPricing : BaseEntity
    {
        [Required]
        public Guid OfferCombinationPricingId { get; set; }

        /// <summary>
        /// Minimum quantity for this tier
        /// </summary>
        [Required]
        public int MinQuantity { get; set; }

        /// <summary>
        /// Maximum quantity (NULL = unlimited)
        /// </summary>
        public int? MaxQuantity { get; set; }

        /// <summary>
        /// Price per unit at this tier
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerUnit { get; set; }
        
        /// <summary>
        /// Sales Price per unit at this tier
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPricePerUnit { get; set; }

        /// <summary>
        /// Discount percentage (alternative to PricePerUnit)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentage { get; set; }

        // Navigation Properties
        [ForeignKey("OfferCombinationPricingId")]
        public virtual TbOfferCombinationPricing OfferCombinationPricing { get; set; } = null!;
    }
}
