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

        public string UnitNameAr { get; set; } = string.Empty;
        public string UnitNameEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string UnitName
        => ResourceManager.CurrentLanguage == Language.Arabic ? UnitNameAr : UnitNameEn;

        public string? VideoLink { get; set; }
        public string ThumbnailImage { get; set; } = string.Empty;
        public bool StockStatus { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }

        // Currency information
        public string? CurrencyCode { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? FormattedPrice { get; set; }

        public List<ItemImageViewDto> ItemImages { get; set; } = new();
    }

    public partial class VwItemDto : ICurrencyConvertible
    {
        public string GetId() => Id.ToString();
        public decimal GetPrice() => Price;

        public async Task ApplyCurrencyConversionAsync(CurrencyConversionDto conversion, string toCurrency)
        {
            Price = conversion.ConvertedAmount;
            FormattedPrice = conversion.FormattedConvertedAmount;
        }

        public void SetCurrencyInfo(string currencyCode, CultureInfo culture)
        {
            CurrencyCode = currencyCode;
            CurrencySymbol = culture.NumberFormat.CurrencySymbol;
        }
    }
}