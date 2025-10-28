using Shared.DTOs.Base;

namespace Shared.DTOs.Notification
{
    public class UserNotificationDto : BaseDto
    {
        public string UserId { get; set; }
        public Guid NotificationId { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
