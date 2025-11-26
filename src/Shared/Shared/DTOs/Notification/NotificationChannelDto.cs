using Common.Enumerations.Notification;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Notification
{
    public class NotificationChannelDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        public NotificationChannel Channel { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Configuration { get; set; }

        public string Title => System.Globalization.CultureInfo.CurrentCulture.Name.StartsWith("ar") 
            ? TitleAr 
            : TitleEn;
    }
}
