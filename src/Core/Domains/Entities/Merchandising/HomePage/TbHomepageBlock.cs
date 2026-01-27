using Common.Enumerations.Merchandising;
using Domains.Entities.Campaign;
using Domains.Entities.Merchandising.HomePageBlocks;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Merchandising.HomePage
{
    public class TbHomepageBlock : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        public HomepageBlockType Type { get; set; }

        public DynamicBlockSource? DynamicSource { get; set; }

        // === Source (لو Personalized) ===
        public PersonalizationSource? PersonalizationSource { get; set; }

        // === Campaign (لو Type = Campaign) ===
        public Guid? CampaignId { get; set; }

        // === Visual Settings (الفروقات الشكلية) ===
        public BlockLayout Layout { get; set; }

        // === Business Rules ===
        public int DisplayOrder { get; set; }

        // === Time-based (للعروض المؤقتة) ===
        public DateTime? VisibleFrom { get; set; }
        public DateTime? VisibleTo { get; set; }

        [StringLength(500)]
        public string? SubtitleAr { get; set; }

        [StringLength(500)]
        public string? SubtitleEn { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool ShowViewAllLink { get; set; } = true;

        [StringLength(200)]
        public string? ViewAllLinkTitleAr { get; set; }

        [StringLength(200)]
        public string? ViewAllLinkTitleEn { get; set; }

        // === Products (لو Manual) ===
        public ICollection<TbBlockItem> BlockItems { get; set; }

        // === Categories (لو CategoryShowcase) ===
        public ICollection<TbBlockCategory> BlockCategories { get; set; }
        public virtual TbCampaign? Campaign { get; set; }
    }
}
