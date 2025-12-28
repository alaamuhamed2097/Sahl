namespace Shared.DTOs.Order.OrderProcessing
{
    public class OrderListItemDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = null!;
        public decimal Total { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
