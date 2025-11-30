using Common.Enumerations;

namespace Shared.GeneralModels.ResultModels
{
    public class CouponCodeValidationResult
    {
        public Guid CouponCodeId { get; set; }
        public string Code { get; set; }
        public decimal DiscountValue { get; set; }
        public CouponCodeType DiscountType { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public List<Guid> ApplicableItems { get; set; } = new List<Guid>();
    }
}
