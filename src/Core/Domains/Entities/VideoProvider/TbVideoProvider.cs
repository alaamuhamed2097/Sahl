using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.VideoProvider
{
    public class TbVideoProvider : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;
    }
}
