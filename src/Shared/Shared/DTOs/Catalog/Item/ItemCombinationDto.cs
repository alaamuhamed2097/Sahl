using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Catalog.Item
{
    public class ItemCombinationDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public bool IsDefault { get; set; } = false;
        public List<CombinationAttributeDto> CombinationAttributes { get; set; } = new List<CombinationAttributeDto>();
        public List<ItemCombinationImageDto> ItemCombinationImages { get; set; } = new List<ItemCombinationImageDto>();
    }

}