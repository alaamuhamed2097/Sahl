using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Notification
{
    public class TbNotification : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; }

        [Required]
        [MaxLength(200)]
        public string TitleEn { get; set; }

        [Required]
        [MaxLength(1000)]
        public string DescriptionAr { get; set; }

        [Required]
        [MaxLength(1000)]
        public string DescriptionEn { get; set; }

        public virtual ICollection<TbUserNotification> TbUserNotification { get; set; }
    }
}