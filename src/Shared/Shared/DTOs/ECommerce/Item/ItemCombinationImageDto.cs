using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemCombinationImageDto : BaseDto
    {
        public string Path { get; set; } = null!;

        public int Order { get; set; }

        public Guid ItemCombinationId { get; set; }
    }
}