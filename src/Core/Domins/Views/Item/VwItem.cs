using System.ComponentModel.DataAnnotations.Schema;

namespace Domins.Views.Item
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
        public bool StockStatus { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string? VideoLink { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }

        [Column("ItemImagesJson")]
        public string? ItemImagesJson { get; set; }
    }
}
