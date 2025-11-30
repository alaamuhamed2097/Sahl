using Common.Enumerations.Merchandising;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Merchandising
{
    public class TbHomepageBlock : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [Required]
        public HomepageBlockType BlockType { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsVisible { get; set; } = true;

        [StringLength(50)]
        public string? BackgroundColor { get; set; }

        [StringLength(500)]
        public string? BackgroundImagePath { get; set; }

        [StringLength(50)]
        public string? TextColor { get; set; }

        [StringLength(100)]
        public string? CssClass { get; set; }

        public int? MaxItemsToDisplay { get; set; }

        public bool ShowViewAllLink { get; set; } = true;

        [StringLength(200)]
        public string? ViewAllLinkUrl { get; set; }

        public DateTime? ActiveFrom { get; set; }

        public DateTime? ActiveTo { get; set; }

        public ICollection<TbBlockProduct> BlockProducts { get; set; } = new HashSet<TbBlockProduct>();
    }
}
