using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Catalog.Item
{
    public class ItemImageDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "ImageRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Path { get; set; } = null!;

        public int Order { get; set; }

        public Guid ItemId { get; set; }
        public bool IsNew { get; set; }
    }
}
