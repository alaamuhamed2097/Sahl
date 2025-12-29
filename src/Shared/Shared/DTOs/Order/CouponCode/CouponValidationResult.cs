namespace Shared.DTOs.Order.CouponCode
{
    public class CouponValidationResult
    {
        public bool IsValid { get; set; }
        public Guid? CouponId { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string DiscountTypeName { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
    }
}
