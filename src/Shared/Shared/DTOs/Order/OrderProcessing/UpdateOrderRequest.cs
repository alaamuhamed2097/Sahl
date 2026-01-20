namespace Shared.DTOs.Order.OrderProcessing
{
    public class UpdateOrderRequest
    {
        public Guid OrderId { get; set; }
        public DateTime? OrderDeliveryDate { get; set; }
        public string? Notes { get; set; }
    }
}
