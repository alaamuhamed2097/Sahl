using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Content
{
    public class TbMediaContent : BaseEntity
    {
        [ForeignKey("ContentArea")]
        public Guid ContentAreaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? DescriptionAr { get; set; }

        [MaxLength(500)]
        public string? DescriptionEn { get; set; }

        [MaxLength(20)]
        public string MediaType { get; set; } = "Image"; // Image, Video, Banner

        [MaxLength(500)]
        public string? MediaPath { get; set; }

        [MaxLength(500)]
        public string? LinkUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual TbContentArea ContentArea { get; set; } = null!;
    }
}
