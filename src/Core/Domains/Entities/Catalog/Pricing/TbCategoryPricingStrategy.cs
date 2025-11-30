using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enumerations.Pricing;

namespace Domains.Entities.Catalog.Pricing
{
    public class TbCategoryPricingStrategy : BaseEntity
    {
        [Required]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Pricing Strategy Type:
        /// </summary>
        [Required]
        public PricingStrategyType PricingStrategyType { get; set; }

        /// <summary>
        /// Allow sellers to override prices?
        /// </summary>
        public bool AllowSellerPricing { get; set; } = true;

        /// <summary>
        /// Require combination attributes for items in this category?
        /// </summary>
        public bool RequireCombinations { get; set; }

        /// <summary>
        /// Enable quantity tiers?
        /// </summary>
        public bool EnableQuantityTiers { get; set; }

        /// <summary>
        /// Configuration JSON for advanced settings
        /// </summary>
        public string? ConfigurationJson { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Domains.Entities.Catalog.Category.TbCategory Category { get; set; } = null!;
    }
}
