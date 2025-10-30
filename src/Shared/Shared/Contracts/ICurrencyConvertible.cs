using Shared.DTOs.Currency;
using System.Globalization;

namespace Shared.Contracts
{
    public interface ICurrencyConvertible
    {
        string GetId();
        decimal GetPrice();
        Task ApplyCurrencyConversionAsync(CurrencyConversionDto conversion, string toCurrency);
        void SetCurrencyInfo(string currencyCode, CultureInfo culture);
    }
}