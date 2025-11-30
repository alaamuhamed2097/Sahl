using System.ComponentModel.DataAnnotations;
using Common.Enumerations.Notification;

namespace Domains.Entities.Notification
{
    public class TbNotificationChannel : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        public NotificationChannel Channel { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? Configuration { get; set; }
    }
}
