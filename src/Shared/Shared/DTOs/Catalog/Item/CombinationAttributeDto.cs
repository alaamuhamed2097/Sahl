using Shared.DTOs.Base;

namespace Shared.DTOs.Catalog.Item
{
    public class CombinationAttributeDto : BaseDto
    {
        public Guid ItemCombinationId { get; set; }
        public Guid AttributeValueId { get; set; }
        public CombinationAttributeValueDto  combinationAttributeValue { get; set; } = new CombinationAttributeValueDto();
    }
}
