using System.Globalization;

namespace Shared.DTOs.Currency
{
    public class CurrencyConversionDto
    {
        public string FromCurrency { get; set; } = string.Empty;
        public string ToCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ConversionDate { get; set; }
        public string FormattedAmount { get; set; } = string.Empty;
        public string FormattedConvertedAmount { get; set; } = string.Empty;
        public CultureInfo Culture { get; set; } = null!;
    }
}