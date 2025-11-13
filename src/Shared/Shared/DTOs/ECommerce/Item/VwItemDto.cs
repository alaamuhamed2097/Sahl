using Resources;
using Resources.Enumerations;
using Shared.Contracts;
using Shared.DTOs.Base;
using Shared.DTOs.Currency;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Item
{
    public partial class VwItemDto : BaseDto
    {
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string Title
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        public string ShortDescriptionAr { get; set; } = string.Empty;
        public string ShortDescriptionEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string ShortDescription
        => ResourceManager.CurrentLanguage == Language.Arabic ? ShortDescriptionAr : ShortDescriptionEn;

        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string Description
        => ResourceManager.CurrentLanguage == Language.Arabic ? DescriptionAr : DescriptionEn;

        public string CategoryTitleAr { get; set; } = string.Empty;
        public string CategoryTitleEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string CategoryTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? CategoryTitleAr : CategoryTitleEn;

        public string UnitTitleAr { get; set; } = string.Empty;
        public string UnitTitleEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string UnitTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? UnitTitleAr : UnitTitleEn;

        public string? VideoLink { get; set; }
        public string ThumbnailImage { get; set; } = string.Empty;
        public bool StockStatus { get; set; }

        /// <summary>
        /// Default quantity from the default combination
        /// </summary>
        public int DefaultQuantity { get; set; }

        /// <summary>
        /// Default price from the default combination
        /// </summary>
        public decimal DefaultPrice { get; set; }

        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }

        // Currency information
        public string? CurrencyCode { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? FormattedPrice { get; set; }

        public List<ItemImageViewDto> ItemImages { get; set; } = new();

        /// <summary>
        /// List of all pricing combinations for this item
        /// </summary>
        public List<ItemCombinationDto> Combinations { get; set; } = new();

        /// <summary>
        /// Backward compatibility - returns DefaultQuantity
        /// </summary>
        [JsonIgnore]
        public int Quantity => DefaultQuantity;

        /// <summary>
        /// Backward compatibility - returns DefaultPrice
        /// </summary>
        [JsonIgnore]
        public decimal Price => DefaultPrice;
    }

    public partial class VwItemDto : ICurrencyConvertible
    {
        public string GetId() => Id.ToString();
        public decimal GetPrice() => DefaultPrice;

        public async Task ApplyCurrencyConversionAsync(CurrencyConversionDto conversion, string toCurrency)
        {
            DefaultPrice = conversion.ConvertedAmount;
            FormattedPrice = conversion.FormattedConvertedAmount;

            // Convert all combinations
            if (Combinations?.Any() == true)
            {
                foreach (var combo in Combinations)
                {
                    combo.Price *= conversion.ExchangeRate;
                    combo.SalesPrice *= conversion.ExchangeRate;
                }
            }
        }

        public void SetCurrencyInfo(string currencyCode, CultureInfo culture)
        {
            CurrencyCode = currencyCode;
            CurrencySymbol = culture.NumberFormat.CurrencySymbol;
        }
    }

    /// <summary>
    /// Represents a pricing combination for an item in the view
    /// </summary>
    public class ItemCombinationDto
    {
        public string AttributeIds { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsDefault { get; set; }
    }
}