using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Merchandising.HomePage
{
    public class TbHomePageSlider : BaseEntity
    {
        [MaxLength(200)]
        public string? TitleAr { get; set; }

        [MaxLength(200)]
        public string? TitleEn { get; set; }
        [Required]
        public string ImageUrl { get; set; } = null!;
        [Required]
        public int DisplayOrder { get; set; }
    }
}
