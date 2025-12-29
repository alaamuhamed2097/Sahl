using Shared.Contracts;
using Shared.DTOs.Currency;

namespace BL.Contracts.GeneralService.Location;

public interface ILocationBasedCurrencyService
{
    Task<(CurrencyDto? targetCurrency, CurrencyDto? baseCurrency)> GetCurrencyInfoAsync(string clientIp);
    Task<T> ApplyCurrencyConversionAsync<T>(T item, string? fromCurrency, string? toCurrency) where T : ICurrencyConvertible;
    Task<IEnumerable<T>> ApplyCurrencyConversionAsync<T>(IEnumerable<T> items, string? fromCurrency, string? toCurrency) where T : ICurrencyConvertible;
}