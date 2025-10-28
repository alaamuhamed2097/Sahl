using Common.Enumerations.Notification;

namespace Shared.GeneralModels.Parameters.Notification
{
    public class NotificationRequest
    {
        public string Recipient { get; set; } // Email, Mobile, or Device Token
        public NotificationChannel Channel { get; set; }
        public NotificationType Type { get; set; }
        public string Subject { get; set; } // For email
        public string Title { get; set; } // For Firebase notifications
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public string CustomTemplate { get; set; } // For custom templates
    }
}
