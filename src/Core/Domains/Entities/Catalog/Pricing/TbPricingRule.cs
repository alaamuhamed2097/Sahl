using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Pricing
{
    public class TbPricingRule : BaseEntity
    {
        [Required]
        public Guid ItemId { get; set; }

        /// <summary>
        /// If null, applies to all combinations
        /// </summary>
        public Guid? ItemCombinationId { get; set; }

        /// <summary>
        /// Rule type: 1=FixedAmount, 2=Percentage, 3=Formula
        /// </summary>
        [Required]
        public int RuleType { get; set; }

        /// <summary>
        /// Modifier value (e.g., +500, -10%, etc.)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ModifierValue { get; set; }

        /// <summary>
        /// Priority (higher = applied first)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Condition: AttributeValue, DateRange, Quantity, etc.
        /// </summary>
        public string? Condition { get; set; }

        [StringLength(200)]
        public string? NameAr { get; set; }

        [StringLength(200)]
        public string? NameEn { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("ItemId")]
        public virtual Domains.Entities.Catalog.Item.TbItem Item { get; set; }

        [ForeignKey("ItemCombinationId")]
        public virtual Domains.Entities.Catalog.Item.ItemAttributes.TbItemCombination? ItemCombination { get; set; }
    }
}
