using Common.Enumerations.Notification;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Notification
{
    public class NotificationsDto
    {
        public Guid Id { get; set; }

        [Required]
        public int RecipientID { get; set; }

        [Required]
        public RecipientType RecipientType { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public Severity Severity { get; set; } = Severity.Medium;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public string? Data { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadDate { get; set; }

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.Pending;

        [StringLength(500)]
        public string? DeliveryChannel { get; set; }
    }

    public class NotificationPreferencesDto
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public RecipientType UserType { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public bool EnableEmail { get; set; } = true;

        public bool EnableSMS { get; set; } = false;

        public bool EnablePush { get; set; } = true;

        public bool EnableInApp { get; set; } = true;
    }
}
