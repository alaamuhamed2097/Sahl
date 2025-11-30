using Resources;
using Resources.Enumerations;

namespace Shared.DTOs.ECommerce.Item
{
    public class VwCombinationAttributeValueDto
    {
        public Guid CombinationAttributeId { get; set; }
        public Guid AttributeId { get; set; }
        public string AttributeTitleAr { get; set; } = null!;
        public string AttributeTitleEn { get; set; } = null!;
        public string AttributeTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? AttributeTitleAr : AttributeTitleEn;
        public string AttributeValue { get; set; } = string.Empty;
    }
}