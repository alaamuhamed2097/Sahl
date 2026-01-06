namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Item card for homepage blocks
    /// NO DOMAIN REFERENCES - Pure DTO for Blazor compatibility
    /// </summary>
    public class ItemCardDto
    {
        public Guid ItemId { get; set; }
        public Guid? ItemCombinationId { get; set; }

        // Bilingual names
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;

        public string? MainImageUrl { get; set; }
        public decimal? Rating { get; set; }
        public bool IsAvailable { get; set; }
        public bool InStock { get; set; }

        // Bilingual badge
        public string? CampaignBadgeAr { get; set; }
        public string? CampaignBadgeEn { get; set; }
    }
}