namespace Shared.ResultModels
{
    public class UserNotificationResult<T>
    {
        public T Value { get; set; }
        public int TotalCount { get; set; }
        public int UnReadCount { get; set; }
    }
}
