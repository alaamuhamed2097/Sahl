namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// DTO for block category
    /// </summary>
    public class AdminBlockCategoryDto
    {
        public Guid Id { get; set; }
        public Guid HomepageBlockId { get; set; }
        public Guid CategoryId { get; set; }

        public string CategoryNameEn { get; set; } = string.Empty;
        public string CategoryNameAr { get; set; } = string.Empty;

        public string? CategoryImage { get; set; }
        public int DisplayOrder { get; set; }
    }
}
