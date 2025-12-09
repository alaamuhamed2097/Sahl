using BL.Contracts.IMapper;
using BL.Contracts.Service.Currency;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Currency;
using Shared.DTOs.Currency;
using System.Globalization;

namespace BL.Service.Currency
{
    public class CurrencyConversionFactory : ICurrencyConversionFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly ICurrencyService _currencyService;

        // Country to Currency mapping
        private readonly Dictionary<string, string> _countryToCurrency = new()
        {
            {"US", "USD"}, {"GB", "GBP"}, {"EU", "EUR"}, {"CA", "CAD"},
            {"AU", "AUD"}, {"JP", "JPY"}, {"CH", "CHF"}, {"CN", "CNY"},
            {"IN", "INR"}, {"BR", "BRL"}, {"MX", "MXN"}, {"KR", "KRW"},
            {"SG", "SGD"}, {"HK", "HKD"}, {"NO", "NOK"}, {"SE", "SEK"},
            {"DK", "DKK"}, {"PL", "PLN"}, {"CZ", "CZK"}, {"HU", "HUF"},
            {"SA", "SAR"}, {"AE", "AED"}, {"EG", "EGP"}, {"QA", "QAR"},
            {"KW", "KWD"}, {"BH", "BHD"}, {"OM", "OMR"}, {"JO", "JOD"},
            {"LB", "LBP"}, {"MA", "MAD"}, {"TN", "TND"}, {"DZ", "DZD"}
        };

        public CurrencyConversionFactory(IUnitOfWork unitOfWork, IBaseMapper mapper, ICurrencyService currencyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currencyService = currencyService;
        }

        public async Task<CurrencyConversionDto> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return new CurrencyConversionDto
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Amount = amount,
                    ConvertedAmount = amount,
                    ExchangeRate = 1m,
                    ConversionDate = DateTime.UtcNow,
                    FormattedAmount = await FormatCurrencyAsync(amount, fromCurrency),
                    FormattedConvertedAmount = await FormatCurrencyAsync(amount, toCurrency),
                    Culture = GetCultureForCurrency(toCurrency)
                };
            }

            var exchangeRate = await GetExchangeRateAsync(fromCurrency, toCurrency);
            var convertedAmount = amount * exchangeRate;

            return new CurrencyConversionDto
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Amount = amount,
                ConvertedAmount = convertedAmount,
                ExchangeRate = exchangeRate,
                ConversionDate = DateTime.UtcNow,
                FormattedAmount = await FormatCurrencyAsync(amount, fromCurrency),
                FormattedConvertedAmount = await FormatCurrencyAsync(convertedAmount, toCurrency),
                Culture = GetCultureForCurrency(toCurrency)
            };
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            var fromCurrencyEntity = await _unitOfWork.TableRepository<TbCurrency>()
                .FindAsync(x => x.Code == fromCurrency && !x.IsDeleted);

            var toCurrencyEntity = await _unitOfWork.TableRepository<TbCurrency>()
                .FindAsync(x => x.Code == toCurrency && !x.IsDeleted);

            if (fromCurrencyEntity == null || toCurrencyEntity == null)
                return 1m;

            // Convert through base currency
            var baseCurrency = await _currencyService.GetBaseCurrencyAsync();
            if (baseCurrency == null) return 1m;

            if (fromCurrency == baseCurrency.Code)
                return toCurrencyEntity.ExchangeRate;

            if (toCurrency == baseCurrency.Code)
                return 1m / fromCurrencyEntity.ExchangeRate;

            // Convert from -> base -> to
            return toCurrencyEntity.ExchangeRate / fromCurrencyEntity.ExchangeRate;
        }

        public async Task<string> FormatCurrencyAsync(decimal amount, string currencyCode)
        {
            var currency = await _unitOfWork.TableRepository<TbCurrency>()
                .FindAsync(x => x.Code == currencyCode && !x.IsDeleted);

            if (currency == null)
                return amount.ToString("N2");

            try
            {
                var culture = GetCultureForCurrency(currencyCode);
                return amount.ToString("C", culture);
            }
            catch
            {
                return $"{amount:N2} {currency.Symbol}";
            }
        }

        public async Task<CurrencyDto> GetCurrencyByCountryAsync(string countryCode)
        {
            if (_countryToCurrency.TryGetValue(countryCode.ToUpper(), out var currencyCode))
            {
                var currency = await _unitOfWork.TableRepository<TbCurrency>()
                    .FindAsync(x => x.Code == currencyCode && !x.IsDeleted);

                return currency != null ? _mapper.MapModel<TbCurrency, CurrencyDto>(currency) : null;
            }

            return null;
        }

        public async Task<IEnumerable<CurrencyDto>> GetSupportedCurrenciesAsync()
        {
            return await _currencyService.GetActiveCurrenciesAsync();
        }

        private CultureInfo GetCultureForCurrency(string currencyCode)
        {
            var cultureMap = new Dictionary<string, string>
            {
                {"USD", "en-US"}, {"EUR", "en-IE"}, {"GBP", "en-GB"},
                {"CAD", "en-CA"}, {"AUD", "en-AU"}, {"JPY", "ja-JP"},
                {"CHF", "de-CH"}, {"CNY", "zh-CN"}, {"INR", "hi-IN"},
                {"SAR", "ar-SA"}, {"AED", "ar-AE"}, {"EGP", "ar-EG"}
            };

            if (cultureMap.TryGetValue(currencyCode, out var cultureName))
            {
                return new CultureInfo(cultureName);
            }

            return CultureInfo.InvariantCulture;
        }
    }
}