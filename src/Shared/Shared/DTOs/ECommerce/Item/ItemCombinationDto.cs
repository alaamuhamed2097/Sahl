using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemCombinationDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public string? Barcode { get; set; }
        public string? SKU { get; set; }
        public bool IsDefault { get; set; } = false;
        public List<CombinationAttributeDto> CombinationAttributes { get; set; } = new List<CombinationAttributeDto>();
        public List<ItemCombinationImageDto> ItemCombinationImages { get; set; } = new List<ItemCombinationImageDto>();
    }
}