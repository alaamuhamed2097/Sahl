namespace Shared.DTOs.Order.OrderProcessing.AdminDashboardOrder
{
    public class OrderTimelineEventDto
    {
        public DateTime EventDate { get; set; }
        public string EventType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PerformedBy { get; set; }
    }
}
