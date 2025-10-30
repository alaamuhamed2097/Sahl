using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domins.Entities.Unit
{
    public class TbUnit : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;
    }
}
