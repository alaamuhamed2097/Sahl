using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Content
{
    public class ContentAreaDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AreaCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public string Title => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") 
            ? TitleAr 
            : TitleEn;

        public string? Description => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") 
            ? DescriptionAr 
            : DescriptionEn;
    }

    public class MediaContentDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ContentAreaId { get; set; }

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(20)]
        public string MediaType { get; set; } = "Image";

        [StringLength(500)]
        public string? MediaPath { get; set; }

        [StringLength(500)]
        public string? LinkUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Title => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") 
            ? TitleAr 
            : TitleEn;

        public string? Description => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") 
            ? DescriptionAr 
            : DescriptionEn;

        public string? ContentAreaTitle { get; set; }
    }
}
