using Common.Enumerations.Visibility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domains.Views.Item
{
    public class VwItem
    {
        public Guid Id { get; set; }
        public string SEOTitle { get; set; } = null!;
        public string SEODescription { get; set; } = null!;
        public string SEOMetaTags { get; set; } = null!;
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
        public Guid BrandId { get; set; }
        public string BrandTitleAr { get; set; }
        public string BrandTitleEn { get; set; }
        public Guid? VideoProviderId { get; set; }
        public string? VideoProviderTitleAr { get; set; }
        public string? VideoProviderTitleEn { get; set; }
        public string? VideoUrl { get; set; }
        public string ThumbnailImage { get; set; }
        public string? Barcode { get; set; } 
        public string? SKU { get; set; } 
        public decimal? BasePrice { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public ProductVisibilityStatus VisibilityScope { get; set; }

        public string? Images { get; set; }
        public string? ItemAttributes { get; set; }
    }
}