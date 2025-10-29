namespace Domins.Views
{
    public class VwCategoryWithAttributes
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsFinal { get; set; }
        public string? ImageUrl { get; set; }
        public string? Icon { get; set; }
        public string? TreeViewSerial { get; set; }
        public int DisplayOrder { get; set; }
        public bool PriceRequired { get; set; }
        public bool IsFeaturedCategory { get; set; }
        public bool IsHomeCategory { get; set; }
        public bool IsMainCategory { get; set; }
        public string? AttributesJson { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
