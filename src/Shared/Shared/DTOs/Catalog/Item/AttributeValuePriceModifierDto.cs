using Common.Enumerations.Pricing;
using Shared.DTOs.Base;

namespace Shared.DTOs.Catalog.Item
{
    public class AttributeValuePriceModifierDto : BaseDto
    {
        public Guid CombinationAttributeValueId { get; set; }
        public Guid AttributeId { get; set; }
        public PriceModifierType ModifierType { get; set; }
        public PriceModifierCategory PriceModifierCategory { get; set; }
        public decimal ModifierValue { get; set; }
        public int DisplayOrder { get; set; }
    }
}
