using Common.Enumerations.Notification;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Notification
{
    /// <summary>
    /// Represents a notification that can be sent through multiple channels
    /// </summary>
    public class TbNotification : BaseEntity
    {
        [Required]
        public int RecipientID { get; set; }

        [Required]
        public RecipientType RecipientType { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public Severity Severity { get; set; } = Severity.Medium;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public string? Data { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadDate { get; set; }

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.Pending;

        [MaxLength(500)]
        public string? DeliveryChannel { get; set; }

        // Navigation collection for related user notifications
        public virtual ICollection<TbUserNotification> TbUserNotification { get; set; } = new HashSet<TbUserNotification>();
    }
}
