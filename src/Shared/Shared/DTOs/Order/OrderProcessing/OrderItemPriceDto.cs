using Shared.DTOs.Base;

namespace Shared.DTOs.Order.OrderProcessing
{
    public class OrderItemPriceDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
