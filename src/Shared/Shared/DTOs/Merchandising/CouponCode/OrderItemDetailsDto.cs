namespace Shared.DTOs.Merchandising.CouponCode
{
    public class OrderItemDetailsDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? CategoryId { get; set; }
        public int? VendorId { get; set; }
    }

}
