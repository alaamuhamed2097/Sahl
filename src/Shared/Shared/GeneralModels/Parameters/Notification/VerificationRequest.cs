using Common.Enumerations.Notification;

namespace Shared.GeneralModels.Parameters.Notification
{
    public class VerificationRequest
    {
        public string Recipient { get; set; } // Email, Mobile, or Device Token
        public NotificationChannel Channel { get; set; }
        public string Subject { get; set; } // For email
        public string Template { get; set; } // Template name/type
        public string Title { get; set; } // For Firebase notifications
    }
}
