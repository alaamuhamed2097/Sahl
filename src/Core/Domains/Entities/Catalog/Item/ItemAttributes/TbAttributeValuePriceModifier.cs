using Common.Enumerations.Pricing;
using Domains.Entities.Catalog.Attribute;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
    public class TbAttributeValuePriceModifier : BaseEntity
    {
        [Required]
        public Guid CombinationAttributeValueId { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        [Required]
        public PriceModifierType ModifierType { get; set; }

        public decimal ModifierValue { get; set; }
        public int DisplayOrder { get; set; }
        // Navigation Properties
        [ForeignKey("CombinationAttributeValueId")]
        public virtual TbCombinationAttributesValue CombinationAttributesValue { get; set; } = null!;

        [ForeignKey("AttributeId")]
        public virtual TbAttribute Attribute { get; set; } = null!;
    }
    
}
