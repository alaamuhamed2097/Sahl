using Resources;
using Resources.Enumerations;
using Shared.Attributes;
using Shared.Contracts;
using Shared.DTOs.Base;
using Shared.DTOs.Currency;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemDto : BaseSeoDto, ICurrencyConvertible
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string Title
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "ShortDescriptionArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ShortDescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "ShortDescriptionEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ShortDescriptionEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string ShortDescription
        => ResourceManager.CurrentLanguage == Language.Arabic ? ShortDescriptionAr : ShortDescriptionEn;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "DescriptionArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "DescriptionEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string DescriptionEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string Description
        => ResourceManager.CurrentLanguage == Language.Arabic ? DescriptionAr : DescriptionEn;

        public string? CategoryTitleAr { get; set; }

        public string? CategoryTitleEn { get; set; }

        [JsonIgnore]
        public string? CategoryTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? CategoryTitleAr : CategoryTitleEn;

        [NotEmptyGuid]
        public Guid CategoryId { get; set; }

        [NotEmptyGuid]
        public Guid BrandId { get; set; }

        [NotEmptyGuid]
        public Guid UnitId { get; set; }

        public Guid? VideoProviderId { get; set; }
        public string? VideoLink { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ThumbnailImage { get; set; } = null!;


        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public bool StockStatus { get; set; }

        public bool IsNewArrival { get; set; } = false;
        public bool IsBestSeller { get; set; } = false;
        public bool IsRecommended { get; set; } = false;

        // Currency information (populated after conversion)
        public string? CurrencyCode { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? FormattedPrice { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? OriginalPrice { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(1, ErrorMessageResourceName = "ImagesRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public List<ItemImageDto> Images { get; set; } = new();

        public List<ItemAttributeDto> ItemAttributes { get; set; } = new();
        public List<ItemAttributeCombinationPricingDto> ItemAttributeCombinationPricings { get; set; } = new();

        /// <summary>
        /// Get price from default combination or first combination
        /// </summary>
        public decimal GetPrice()
        {
            if (ItemAttributeCombinationPricings?.Any() == true)
            {
                var defaultCombination = ItemAttributeCombinationPricings.FirstOrDefault(c => c.IsDefault);
                return defaultCombination?.Price ?? ItemAttributeCombinationPricings.First().Price;
            }

            return 0;
        }

        public string GetId() => Id.ToString();

        public async Task ApplyCurrencyConversionAsync(CurrencyConversionDto conversion, string toCurrency)
        {
            // Convert attribute pricings if they exist
            if (ItemAttributeCombinationPricings?.Any() == true)
            {
                foreach (var pricing in ItemAttributeCombinationPricings)
                {
                    // Apply conversion to both Price and SalesPrice
                    pricing.Price *= conversion.ExchangeRate;
                    pricing.SalesPrice *= conversion.ExchangeRate;
                }
            }

            ExchangeRate = conversion.ExchangeRate;
            OriginalPrice = conversion.Amount;
            FormattedPrice = conversion.FormattedConvertedAmount;
        }

        public void SetCurrencyInfo(string currencyCode, CultureInfo culture)
        {
            CurrencyCode = currencyCode;
            CurrencySymbol = culture.NumberFormat.CurrencySymbol;
        }
    }
}
