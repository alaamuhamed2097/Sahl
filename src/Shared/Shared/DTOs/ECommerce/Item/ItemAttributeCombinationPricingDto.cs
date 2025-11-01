using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemAttributeCombinationPricingDto : BaseDto
    {
        public Guid ItemId { get; set; }

        public string AttributeIds { get; set; } = null!;

        public decimal FinalPrice { get; set; }

        public int Quantity { get; set; }

        [Required(ErrorMessageResourceName = "ImageRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? Image { get; set; }
    }
}
