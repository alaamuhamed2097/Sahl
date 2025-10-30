using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemAttributeDto : BaseDto
    {
        public Guid ItemId { get; set; }

        public Guid AttributeId { get; set; }

        public string Value { get; set; } = null!;
    }
}
