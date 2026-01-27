namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Homepage block DTO
    /// </summary>
    public class HomepageBlockDto
    {
        public Guid Id { get; set; }

        // Bilingual titles
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string? SubtitleAr { get; set; }
        public string? SubtitleEn { get; set; }

        public string Type { get; set; } = string.Empty;
        public string Layout { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool ShowViewAllLink { get; set; }

        // Bilingual view all link
        public string? ViewAllLinkTitleAr { get; set; }
        public string? ViewAllLinkTitleEn { get; set; }

        // Campaign info (if block is campaign type)
        public CampaignInfoDto? Campaign { get; set; }

        // Content
        public List<ItemCardDto> Products { get; set; } = new();
        public List<CategoryCardDto> Categories { get; set; } = new();

        // Counts
        public int TotalProductCount { get; set; }
        public int? TotalCategoryCount { get; set; }
    }
}