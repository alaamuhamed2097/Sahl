using Shared.DTOs.Currency;

namespace BL.Contracts.Service.Currency;

public interface ICurrencyConversionFactory
{
    Task<CurrencyConversionDto> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
    Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    Task<string> FormatCurrencyAsync(decimal amount, string currencyCode);
    Task<CurrencyDto> GetCurrencyByCountryAsync(string countryCode);
    Task<IEnumerable<CurrencyDto>> GetSupportedCurrenciesAsync();
}