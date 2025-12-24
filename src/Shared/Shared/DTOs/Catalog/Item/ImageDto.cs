using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Item
{
    public class ImageDto : BaseDto
    {
        public string Path { get; set; } = null!;
        public int Order { get; set; }
        public bool IsDefault { get; set; }
    }
}
