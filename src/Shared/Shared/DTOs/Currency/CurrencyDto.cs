using Resources;
using Resources.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Currency
{
    /// <summary>
    /// Data Transfer Object for Currency entity
    /// Contains currency information including code, names, symbol, and exchange rate
    /// </summary>
    public class CurrencyDto
    {
        /// <summary>
        /// Unique identifier for the currency
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ISO 4217 currency code (e.g., USD, EUR, SAR)
        /// Must be exactly 3 characters
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(3, MinimumLength = 3, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Currency name in English (e.g., US Dollar, Euro, Saudi Riyal)
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string NameEn { get; set; } = string.Empty;

        /// <summary>
        /// Currency name in Arabic (e.g., دولار أمريكي, يورو, ريال سعودي)
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string NameAr { get; set; } = string.Empty;

        [JsonIgnore]
        public string Name
        => ResourceManager.CurrentLanguage == Language.Arabic ? NameAr : NameEn;

        /// <summary>
        /// Currency symbol (e.g., $, €, ر.س)
        /// Maximum 5 characters
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(5, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Exchange rate relative to the base currency
        /// Must be a positive decimal value greater than 0.0001
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [Range(0.0001, double.MaxValue, ErrorMessageResourceName = "PositiveRange", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal ExchangeRate { get; set; } = 1m;

        /// <summary>
        /// Indicates if this currency is the base currency for exchange rate calculations
        /// Only one currency can be the base currency at a time
        /// </summary>
        public bool IsBaseCurrency { get; set; }

        /// <summary>
        /// Indicates if the currency is active and available for use
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ISO 3166-1 alpha-2 country code (e.g., US, DE, SA)
        /// Optional field for associating currency with a country
        /// </summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the last exchange rate update
        /// Automatically set when exchange rates are updated
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Formatted display name combining code and English name
        /// Read-only property for UI display purposes
        /// </summary>
        public string DisplayName => $"{Code} - {NameEn}";

        /// <summary>
        /// Formatted currency symbol for display
        /// Read-only property that returns the symbol as-is
        /// </summary>
        public string FormattedSymbol => $"{Symbol}";
    }
}