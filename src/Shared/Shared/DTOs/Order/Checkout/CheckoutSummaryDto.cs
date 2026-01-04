using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.CouponCode;

namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Comprehensive checkout summary with all calculations
    /// </summary>
    public class CheckoutSummaryDto
    {
        public List<CheckoutItemDto> Items { get; set; } = new();
        //public List<ShipmentPreviewDto> ShipmentPreviews { get; set; } = new();
        public AddressSelectionDto? DeliveryAddress { get; set; }
        public PriceBreakdownDto PriceBreakdown { get; set; } = new();
        public CouponInfoDto? CouponInfo { get; set; }
        public Guid? CouponId { get; set; }
    }
}