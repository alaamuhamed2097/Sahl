using Shared.DTOs.Base;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Catalog.Item
{
    public class ItemSectionDto : BaseDto
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string ThumbnailImage { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public decimal? Price { get; set; }
        public bool PriceRequired { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        //public string TimeAgo => CreatedDateUtc.GetTimeAgo();
        public string Location { get; set; } = null!;
        [JsonIgnore]
        public int ViewCount { get; set; }
        public bool IsSponsored { get; set; }
        public Guid ItemCurrencyId { get; set; }
        //public CurrencyInfoDto CurrencyInfo { get; set; }
    }
}
