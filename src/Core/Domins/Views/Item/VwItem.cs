using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

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

        /// <summary>
        /// Default price from the default combination (IsDefault = true)
        /// </summary>
        public decimal DefaultPrice { get; set; }

        /// <summary>
        /// Default quantity from the default combination (IsDefault = true)
        /// </summary>
        public int DefaultQuantity { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public string? VideoLink { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }

        [Column("ItemImagesJson")]
        public string? ItemImagesJson { get; set; }

        /// <summary>
        /// JSON string containing all pricing combinations
        /// Format: [{"AttributeIds":"guid1,guid2","Price":100,"SalesPrice":90,"Quantity":50,"IsDefault":true}]
        /// </summary>
        [Column("CombinationsJson")]
        public string? CombinationsJson { get; set; }

        /// <summary>
        /// Parsed list of pricing combinations
        /// </summary>
        [NotMapped]
        public List<ItemCombination>? Combinations
        {
            get
            {
                if (string.IsNullOrEmpty(CombinationsJson))
                    return new List<ItemCombination>();

                try
                {
                    return JsonSerializer.Deserialize<List<ItemCombination>>(CombinationsJson);
                }
                catch
                {
                    return new List<ItemCombination>();
                }
            }
        }
    }

    /// <summary>
    /// Represents a pricing combination for an item
    /// </summary>
    public class ItemCombination
    {
        public string AttributeIds { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsDefault { get; set; }
    }
}
