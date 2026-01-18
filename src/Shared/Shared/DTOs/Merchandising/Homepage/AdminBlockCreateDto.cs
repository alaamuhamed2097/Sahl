using Common.Enumerations.Merchandising;

namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// DTO for creating/updating a homepage block (Admin)
    /// </summary>
    public class AdminBlockCreateDto
    {
        public Guid? Id { get; set; }

        // Bilingual titles (required)
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;

        // Subtitles (optional)
        public string? SubtitleAr { get; set; }
        public string? SubtitleEn { get; set; }

        // Block type (Manual, Dynamic, Campaign, Personalized)
        public string Type { get; set; } = string.Empty;

        // Visual layout (Carousel, TwoRows, Featured, Compact, FullWidth)
        public BlockLayout Layout { get; set; }

        // Display settings
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool ShowViewAllLink { get; set; } = true;

        // View all link text (optional)
        public string? ViewAllLinkTitleAr { get; set; }
        public string? ViewAllLinkTitleEn { get; set; }

        // Time-based visibility (optional)
        public DateTime? VisibleFrom { get; set; }
        public DateTime? VisibleTo { get; set; }

        // Campaign ID (if Type = Campaign)
        public Guid? CampaignId { get; set; }

        // Dynamic source (if Type = Dynamic)
        public string? DynamicSource { get; set; }

        // Personalization source (if Type = Personalized)
        public string? PersonalizationSource { get; set; }

        // Manual items selection (if Type = ManualItems)
        public List<AdminBlockItemDto> Items { get; set; } = new();

        // Manual categories selection (if Type = ManualCategories)
        public List<AdminBlockCategoryDto> Categories { get; set; } = new();
    }
}
