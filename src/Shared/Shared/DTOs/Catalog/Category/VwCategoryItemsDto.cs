using Shared.DTOs.Base;
using Shared.DTOs.Catalog.Item;

namespace Shared.DTOs.Catalog.Category
{
    public class VwCategoryItemsDto : BaseDto
    {
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsFinal { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public string TreeViewSerial { get; set; }
        public bool IsFeaturedCategory { get; set; }
        public bool IsHomeCategory { get; set; }
        public bool IsMainCategory { get; set; }
        public bool PriceRequired { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public List<ItemSectionDto> Items { get; set; } = new List<ItemSectionDto>();
    }
}
