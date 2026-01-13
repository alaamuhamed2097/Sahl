using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Offer
{
    public class CombinationAttributeDetailsDto :BaseDto
    {
        public Guid AttributeId { get; set; }
        public string AttributeTitleAr { get; set; } = string.Empty;
        public string AttributeTitleEn { get; set; } = string.Empty;
        public int AttributeFieldType { get; set; }
        public string Value { get; set; } = string.Empty;
    }

}