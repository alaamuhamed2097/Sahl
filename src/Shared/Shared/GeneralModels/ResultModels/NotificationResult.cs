namespace Shared.GeneralModels.ResultModels
{
    public class NotificationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime SentAt { get; set; }
        public int TotalAttempted { get; set; }
        public int TotalSent { get; set; }
        public int TotalFailed { get; set; }
    }
}
