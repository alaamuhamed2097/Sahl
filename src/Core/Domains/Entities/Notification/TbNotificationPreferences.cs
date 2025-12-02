using Common.Enumerations.Notification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Notification
{
    /// <summary>
    /// User preferences for receiving notifications
    /// </summary>
    public class TbNotificationPreferences : BaseEntity
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public RecipientType UserType { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public bool EnableEmail { get; set; } = true;

        public bool EnableSMS { get; set; } = false;

        public bool EnablePush { get; set; } = true;

        public bool EnableInApp { get; set; } = true;
        [ForeignKey (nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
