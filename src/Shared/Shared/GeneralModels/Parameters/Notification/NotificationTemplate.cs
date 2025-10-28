using Common.Enumerations.Notification;

namespace Shared.GeneralModels.Parameters.Notification
{
    public class NotificationTemplate
    {
        public string Name { get; set; }
        public NotificationChannel Channel { get; set; }
        public NotificationType Type { get; set; }
        public string Subject { get; set; } // For email
        public string Title { get; set; } // For Firebase
        public string Content { get; set; }
        public Dictionary<string, string> DefaultParameters { get; set; } = new Dictionary<string, string>();
    }
}
