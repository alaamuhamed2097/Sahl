using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Item
{
    public class CombinationAttributeDto : BaseDto
    {
        public Guid ItemCombinationId { get; set; }
        public List<CombinationAttributeValueDto>  combinationAttributeValueDtos { get; set; } = new List<CombinationAttributeValueDto>();
    }
}
