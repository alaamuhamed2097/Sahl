using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Content
{
    public class TbContentArea : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AreaCode { get; set; } = string.Empty; // e.g., "HOME_BANNER", "SIDEBAR"

        [MaxLength(500)]
        public string? DescriptionAr { get; set; }

        [MaxLength(500)]
        public string? DescriptionEn { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public virtual ICollection<TbMediaContent>? MediaContents { get; set; }
    }
}
