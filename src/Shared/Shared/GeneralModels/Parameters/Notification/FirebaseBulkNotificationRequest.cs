namespace Shared.GeneralModels.Parameters.Notification
{
    public class FirebaseBulkNotificationRequest
    {
        public List<string> DeviceTokens { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Sound { get; set; }
        public int? Badge { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}