namespace Shared.GeneralModels.ResultModels
{
    public class AppliedPromoCodeResult
    {
        public decimal OriginalTotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NewTotal { get; set; }

        public Guid PromoCodeId { get; set; }
        public string Code { get; set; }

        public PromoCodeType DiscountType { get; set; }
    }
}
