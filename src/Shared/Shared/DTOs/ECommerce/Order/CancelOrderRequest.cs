namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Cancel Order Request
    /// </summary>
    public class CancelOrderRequest
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; } = null!;
        public string? AdminNotes { get; set; }
    }
}
