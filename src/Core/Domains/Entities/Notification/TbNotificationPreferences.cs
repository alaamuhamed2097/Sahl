using Common.Enumerations.Notification;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Notification
{
    /// <summary>
    /// User preferences for receiving notifications
    /// </summary>
    public class TbNotificationPreferences : BaseEntity
    {
        [Required]
        // FK moved to Fluent configuration. Use Guid to match ApplicationUser primary key.
        public Guid UserId { get; set; }

        [Required]
        public RecipientType UserType { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public bool EnableEmail { get; set; } = true;

        public bool EnableSMS { get; set; } = false;

        public bool EnablePush { get; set; } = true;

        public bool EnableInApp { get; set; } = true;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
