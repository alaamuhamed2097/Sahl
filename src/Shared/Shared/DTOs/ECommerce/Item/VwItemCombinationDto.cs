using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Represents a pricing combination for an item in the view
    /// </summary>
    public class VwItemCombinationDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public Guid ItemCombinationId { get; set; }
        public List<VwCombinationAttributeValueDto> CombinationAttributeValue { get; set; }= new List<VwCombinationAttributeValueDto>();
    }
}