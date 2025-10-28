namespace Shared.GeneralModels.Parameters.Notification
{
    public class FirebaseTopicNotificationRequest
    {
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Sound { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}