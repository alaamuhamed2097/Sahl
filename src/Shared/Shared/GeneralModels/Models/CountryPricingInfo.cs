namespace Bl.GeneralModels.Payment
{
    /// <summary>
    /// Country-specific pricing and payment information
    /// </summary>
    public class CountryPricingInfo
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string Currency { get; set; } = "USD";
        public string CurrencySymbol { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal ConversionFactor { get; set; } = 1.0m;
        public decimal CurrencyRate { get; set; } = 1.0m;
        public decimal DiscountMultiplier { get; set; } = 1.0m;
        //public PaymentGatewayType PaymentGateway { get; set; }
        //public string PaymentGatewayName => PaymentGateway.ToString();
    }
}