using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Currency;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Shared.Contracts;
using Shared.DTOs.Currency;
using System.Globalization;

namespace BL.GeneralService.Location;

public class LocationBasedCurrencyService : ILocationBasedCurrencyService
{
    private readonly IIpGeolocationService _ipGeolocationService;
    private readonly ICurrencyConversionFactory _currencyConversionFactory;
    private readonly ICurrencyService _currencyService;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    // Cache configuration
    private const string CURRENCIES_CACHE_KEY = "active_currencies";
    private const string BASE_CURRENCY_CACHE_KEY = "base_currency";
    private const int CACHE_DURATION_MINUTES = 30;

    public LocationBasedCurrencyService(
        IIpGeolocationService ipGeolocationService,
        ICurrencyConversionFactory currencyConversionFactory,
        ICurrencyService currencyService,
        IMemoryCache cache,
        ILogger logger)
    {
        _ipGeolocationService = ipGeolocationService;
        _currencyConversionFactory = currencyConversionFactory;
        _currencyService = currencyService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<(CurrencyDto? targetCurrency, CurrencyDto? baseCurrency)> GetCurrencyInfoAsync(string clientIp)
    {
        var targetCurrency = await GetTargetCurrencyAsync(clientIp);
        var baseCurrency = await GetBaseCurrencyAsync();
        return (targetCurrency, baseCurrency);
    }

    public async Task<T> ApplyCurrencyConversionAsync<T>(T item, string? fromCurrency, string? toCurrency)
        where T : ICurrencyConvertible
    {
        if (item == null || string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
            return item;

        try
        {
            var conversion = await _currencyConversionFactory.ConvertAsync(item.GetPrice(), fromCurrency, toCurrency);
            await item.ApplyCurrencyConversionAsync(conversion, toCurrency);
            await PopulateCurrencyInfoAsync(item, toCurrency);

            return item;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error converting currency for item {ItemId}", item.GetId());
            return item;
        }
    }

    public async Task<IEnumerable<T>> ApplyCurrencyConversionAsync<T>(IEnumerable<T> items, string? fromCurrency, string? toCurrency)
        where T : ICurrencyConvertible
    {
        if (items?.Any() != true || string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
            return items ?? Enumerable.Empty<T>();

        var convertedItems = new List<T>();
        foreach (var item in items)
        {
            var convertedItem = await ApplyCurrencyConversionAsync(item, fromCurrency, toCurrency);
            convertedItems.Add(convertedItem);
        }

        return convertedItems;
    }

    #region Private Helper Methods

    private async Task<CurrencyDto?> GetBaseCurrencyAsync()
    {
        return await _cache.GetOrCreateAsync(BASE_CURRENCY_CACHE_KEY, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_DURATION_MINUTES);
            return await _currencyService.GetBaseCurrencyAsync();
        });
    }

    private async Task<IEnumerable<CurrencyDto>> GetActiveCurrenciesAsync()
    {
        return await _cache.GetOrCreateAsync(CURRENCIES_CACHE_KEY, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_DURATION_MINUTES);
            return await _currencyService.GetActiveCurrenciesAsync();
        });
    }

    private async Task<CurrencyDto?> GetTargetCurrencyAsync(string clientIp)
    {
        try
        {
            var cacheKey = $"target_currency_{clientIp}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_DURATION_MINUTES);

                var locationResult = await _ipGeolocationService.GetLocationFromIpAsync(clientIp);

                if (locationResult.Success && !string.IsNullOrEmpty(locationResult.CountryCode))
                {
                    var currency = await _currencyConversionFactory.GetCurrencyByCountryAsync(locationResult.CountryCode);
                    if (currency != null) return currency;

                    var targetCurrencyCode = locationResult.CountryCode.ToUpper() == "EG" ? "EGP" : "USD";
                    var allCurrencies = await GetActiveCurrenciesAsync();
                    return allCurrencies.FirstOrDefault(c =>
                        c.Code.Equals(targetCurrencyCode, StringComparison.OrdinalIgnoreCase));
                }

                // Default to USD
                var defaultCurrencies = await GetActiveCurrenciesAsync();
                return defaultCurrencies.FirstOrDefault(c =>
                    c.Code.Equals("USD", StringComparison.OrdinalIgnoreCase));
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error determining target currency for IP {ClientIp}", clientIp);
            return null;
        }
    }

    private static readonly Dictionary<string, CultureInfo> CurrencyCultureCache =
            CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                       .Select(c => new { Culture = c, Region = new RegionInfo(c.Name) })
                       .GroupBy(x => x.Region.ISOCurrencySymbol.ToUpperInvariant())
                       .ToDictionary(g => g.Key, g => g.First().Culture);

    private static CultureInfo GetCultureByCurrencyCode(string currencyCode)
    {
        if (CurrencyCultureCache.TryGetValue(currencyCode.ToUpperInvariant(), out var culture))
            return culture;

        return null;
    }

    private Task PopulateCurrencyInfoAsync<T>(T item, string currencyCode) where T : ICurrencyConvertible
    {
        var culture = GetCultureByCurrencyCode(currencyCode);
        var region = new RegionInfo(culture.Name);

        item.SetCurrencyInfo(region.ISOCurrencySymbol, culture);

        return Task.CompletedTask;
    }

    #endregion
}