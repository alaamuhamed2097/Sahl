using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Item
{
    public class CombinationAttributeValueDto : BaseDto
    {
        public Guid CombinationAttributeId { get; set; }
        public Guid AttributeId { get; set; }
        public string Value { get; set; }
        public List<AttributeValuePriceModifierDto> AttributeValuePriceModifiers { get; set; } = new List<AttributeValuePriceModifierDto>();
    }
}