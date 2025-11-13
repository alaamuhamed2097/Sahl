namespace Shared.GeneralModels.ResultModels
{
    public class PromoCodeValidationResult
    {
        public Guid PromoCodeId { get; set; }
        public string Code { get; set; }
        public decimal DiscountValue { get; set; }
        public PromoCodeType DiscountType { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public List<Guid> ApplicableItems { get; set; } = new List<Guid>();
    }
}
