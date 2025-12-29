namespace Shared.DTOs.Order.OrderEvents
{

    /// <summary>
    /// Order paid event - triggered when payment is successful
    /// </summary>
    public class OrderPaidEvent
    {
        public Guid? OrderId { get; set; }
        public string InvoiceId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}