using Common.Enumerations;

namespace Shared.GeneralModels.ResultModels
{
    public class AppliedCouponCodeResult
    {
        public decimal OriginalTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NewTotal { get; set; }

        public Guid CouponCodeId { get; set; }
        public string Code { get; set; }

        public CouponCodeType DiscountType { get; set; }
    }
}
