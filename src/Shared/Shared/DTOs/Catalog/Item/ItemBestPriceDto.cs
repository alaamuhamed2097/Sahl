namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Best price information for an item across all vendors
    /// </summary>
    public class ItemBestPriceDto
    {
        /// <summary>
        /// Item unique identifier
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Best price available for this item
        /// </summary>
        public decimal BestPrice { get; set; }

        /// <summary>
        /// Total available stock across all vendors
        /// </summary>
        public int TotalStock { get; set; }

        /// <summary>
        /// Total number of offers for this item
        /// </summary>
        public int TotalOffers { get; set; }

        /// <summary>
        /// Buy Box winner ratio (0-1)
        /// </summary>
        public double BuyBoxRatio { get; set; }
    }
}