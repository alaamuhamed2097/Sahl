namespace Shared.GeneralModels.Parameters.Notification
{
    public class FirebaseNotificationRequest
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public string ImageUrl { get; set; }
        public string Sound { get; set; } = "default";
        public int Badge { get; set; }
    }
}
