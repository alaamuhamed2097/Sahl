namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Single item search result with best offer information
    /// </summary>
    public class ItemSearchResultDto
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Product title in Arabic
        /// </summary>
        public string TitleAr { get; set; }

        /// <summary>
        /// Product title in English
        /// </summary>
        public string TitleEn { get; set; }

        /// <summary>
        /// Short description in Arabic
        /// </summary>
        public string ShortDescriptionAr { get; set; }

        /// <summary>
        /// Short description in English
        /// </summary>
        public string ShortDescriptionEn { get; set; }

        /// <summary>
        /// Category ID
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Brand ID (optional)
        /// </summary>
        public Guid? BrandId { get; set; }

        /// <summary>
        /// Product thumbnail image URL
        /// </summary>
        public string ThumbnailImage { get; set; }

        /// <summary>
        /// Item creation date
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// Minimum price across all vendors
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price across all vendors
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Total number of vendor offers for this item
        /// </summary>
        public int OffersCount { get; set; }

        /// <summary>
        /// Fastest delivery time in days among all vendors
        /// </summary>
        public int FastestDelivery { get; set; }

        /// <summary>
        /// Best offer details (usually lowest price with fast delivery)
        /// </summary>
        public BestOfferDto BestOffer { get; set; }
    }
}
