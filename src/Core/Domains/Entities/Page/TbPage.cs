using Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Page
{
    public class TbPage : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        public string ContentEn { get; set; } = string.Empty;

        [Required]
        public string ContentAr { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ShortDescriptionEn { get; set; }

        [StringLength(500)]
        public string? ShortDescriptionAr { get; set; }

        public PageType PageType { get; set; }
    }
}