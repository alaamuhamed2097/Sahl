using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Item
{
    public class CombinationAttributeValueDto : BaseDto
    {
        [Required]
        public Guid CombinationAttributeId { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        [StringLength(200)]
        public string Value { get; set; }
    }
}
