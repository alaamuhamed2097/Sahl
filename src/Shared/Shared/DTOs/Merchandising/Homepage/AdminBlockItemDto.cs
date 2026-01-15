namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// DTO for block product/item
    /// </summary>
    public class AdminBlockItemDto
    {
        public Guid Id { get; set; }
        public Guid HomepageBlockId { get; set; }
        public Guid ItemId { get; set; }

        public string ItemNameEn { get; set; } = string.Empty;
        public string ItemNameAr { get; set; } = string.Empty;

        public string? ItemImage { get; set; }
        public decimal Price { get; set; }
        public int DisplayOrder { get; set; }
    }
}
