using Resources;
using Resources.Enumerations;
using Shared.Contracts;
using Shared.DTOs.Base;
using Shared.DTOs.Currency;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Catalog.Item
{
    public partial class VwItemDto : BaseSeoDto
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

        public Guid CategoryId { get; set; }
        public string CategoryTitleAr { get; set; } = string.Empty;
        public string CategoryTitleEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string CategoryTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? CategoryTitleAr : CategoryTitleEn;

        public Guid UnitId { get; set; }
        public string UnitTitleAr { get; set; } = string.Empty;
        public string UnitTitleEn { get; set; } = string.Empty;

        [JsonIgnore]
        public string UnitTitle
        => ResourceManager.CurrentLanguage == Language.Arabic ? UnitTitleAr : UnitTitleEn;

        public Guid? VideoProviderId { get; set; }
        public string? VideoLink { get; set; }
        public string ThumbnailImage { get; set; } = string.Empty;
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public bool IsNewArrival { get; set; }

        //ByBox Offer
        public Guid? ByBoxOfferId { get; set; }
        public Guid? ByBoxUserId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public List<ItemImageViewDto> ItemImages { get; set; } = new();

        /// <summary>
        /// List of all pricing combinations for this item
        /// </summary>
        public List<VwItemCombinationDto> Combinations { get; set; } = new();
    }

    //public partial class VwItemDto : ICurrencyConvertible
    //{
    //    public string GetId() => Id.ToString();
    //    public decimal GetPrice() => DefaultPrice;

    //    public async Task ApplyCurrencyConversionAsync(CurrencyConversionDto conversion, string toCurrency)
    //    {
    //        DefaultPrice = conversion.ConvertedAmount;
    //        FormattedPrice = conversion.FormattedConvertedAmount;

    //        // Convert all combinations
    //        if (Combinations?.Any() == true)
    //        {
    //            foreach (var combo in Combinations)
    //            {
    //                combo.Price *= conversion.ExchangeRate;
    //                combo.SalesPrice *= conversion.ExchangeRate;
    //            }
    //        }
    //    }

    //    public void SetCurrencyInfo(string currencyCode, CultureInfo culture)
    //    {
    //        CurrencyCode = currencyCode;
    //        CurrencySymbol = culture.NumberFormat.CurrencySymbol;
    //    }
    //}
}