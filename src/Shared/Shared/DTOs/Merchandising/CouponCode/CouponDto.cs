using Common.Enumerations.Order;

namespace Shared.DTOs.Merchandising.CouponCode
{
    /// <summary>
    /// Coupon DTO
    /// </summary>
    public class CouponDto
    {
        public string Code { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string? Description { get; set; }
    }
}
