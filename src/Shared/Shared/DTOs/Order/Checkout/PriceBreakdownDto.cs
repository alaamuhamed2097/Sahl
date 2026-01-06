namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Price breakdown with all components
    /// </summary>
    public class PriceBreakdownDto
    {
        public decimal Subtotal { get; set; }
        public decimal? ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}