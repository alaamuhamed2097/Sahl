using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Category
{
    public class CategoryPreviewDto : BaseDto
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string Title => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;
        public string ImageUrl { get; set; } = null!;
        public string? TreeViewSerial { get; set; }
        public bool PriceRequired { get; set; }
        public List<CategoryAttributeDto>? CategoryAttributes { get; set; } = new();
    }
}
