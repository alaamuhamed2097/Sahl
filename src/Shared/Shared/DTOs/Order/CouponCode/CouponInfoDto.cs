namespace Shared.DTOs.Order.CouponCode
{
    /// <summary>
    /// Coupon information after validation
    /// </summary>
    public class CouponInfoDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string DiscountType { get; set; } = string.Empty;
    }
}