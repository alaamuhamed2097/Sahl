namespace Shared.Events.Order
{
    /// <summary>
    /// Base order event
    /// </summary>
    public class OrderEvent
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
        public string? Reason { get; set; }
    }
}