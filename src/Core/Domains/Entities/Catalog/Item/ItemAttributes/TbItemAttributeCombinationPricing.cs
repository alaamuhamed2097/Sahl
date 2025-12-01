using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
    public class TbItemAttributeCombinationPricing : BaseEntity
    {
        [Required]
        public Guid ItemId { get; set; }

        /// <summary>
        /// Optional: link to a specific ItemCombination (if combinations are stored separately)
        /// </summary>
        public Guid? ItemCombinationId { get; set; }

        /// <summary>
        /// Comma separated list of attribute option ids for compatibility with existing UI
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string AttributeIds { get; set; } = null!;

        /// <summary>
        /// Base price set by admin/system for this combination (reference price)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BasePrice { get; set; }

        /// <summary>
        /// Price (original / list price)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Sales price (final price shown after discounts)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// Cost price for profit calculation
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostPrice { get; set; }

        /// <summary>
        /// Suggested retail price (MSRP)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SuggestedPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Indicates if this is the default pricing combination for the item.
        /// Only one combination per item can be marked as default.
        /// </summary>
        [Required]
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// SKU specific to this combination
        /// </summary>
        [StringLength(100)]
        public string? SKU { get; set; }

        /// <summary>
        /// EAN/Barcode for this specific variant
        /// </summary>
        [StringLength(100)]
        public string? Barcode { get; set; }

        public DateTime? LastPriceUpdate { get; set; }

        // Navigation Properties
        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;

        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination? ItemCombination { get; set; }
    }
}
