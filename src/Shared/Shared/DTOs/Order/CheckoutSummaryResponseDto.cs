using Shared.DTOs.Base;

namespace Shared.DTOs.Order
{
    /// <summary>
    /// Stage 2: Checkout Summary - Preview before creating order
    /// </summary>
    public class CheckoutSummaryResponseDto : BaseDto
    {
        public decimal SubTotal { get; set; }
        public decimal EstimatedShipping { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public List<CartItemPreviewDto> Items { get; set; } = new();
        public List<ExpectedShipmentDto> ExpectedShipments { get; set; } = new();
        public int TotalShipmentsExpected { get; set; }
        public string Message { get; set; } = null!;
    }
}
