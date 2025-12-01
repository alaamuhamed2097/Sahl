using System.ComponentModel.DataAnnotations.Schema;
using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domains.Views.Item
{
    public class VwItem
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryTitleAr { get; set; }
        public string CategoryTitleEn { get; set; }
        public Guid UnitId { get; set; }
        public string UnitTitleAr { get; set; }
        public string UnitTitleEn { get; set; }
        public string ThumbnailImage { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; }
        public Guid? VideoProviderId { get; set; }
        public string? VideoProviderTitleAr { get; set; }
        public string? VideoProviderTitleEn { get; set; }
        public string? VideoLink { get; set; }
        public bool IsNewArrival { get; set; }
        //ByBox Offer
        public Guid? ByBoxOfferId { get; set; }
        public Guid? ByBoxUserId { get; set; }

        public string? ItemImagesJson { get; set; }
        public string? CombinationsJson { get; set; }
    }
}