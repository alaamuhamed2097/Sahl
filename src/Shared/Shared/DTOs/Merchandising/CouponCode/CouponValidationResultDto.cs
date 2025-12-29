using Common.Enumerations.Order;

namespace Shared.DTOs.Merchandising.CouponCode
{
    /// <summary>
    /// DTO for coupon validation result
    /// </summary>
    public class CouponValidationResultDto
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public Guid? CouponId { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
    }

}
