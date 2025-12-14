using System;

namespace Domains.Views.Item
{
    /// <summary>
    /// Denormalized view for fast price lookups
    /// Used for catalog pages and quick price displays
    /// Queries VwItemBestPrices view from the database
    /// </summary>
    public class VwItemBestPrice
    {
        /// <summary>
        /// Item identifier
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Best (lowest) price available for this item
        /// </summary>
        public decimal BestPrice { get; set; }

        /// <summary>
        /// Total stock available across all vendors
        /// </summary>
        public int TotalStock { get; set; }

        /// <summary>
        /// Total number of offers available
        /// </summary>
        public int TotalOffers { get; set; }

        /// <summary>
        /// Whether any vendor offers free shipping
        /// 0 = No free shipping, 1 = Has free shipping
        /// </summary>
        public int HasFreeShipping { get; set; }

        /// <summary>
        /// Fastest delivery time in days
        /// </summary>
        public int FastestDelivery { get; set; }

        /// <summary>
        /// Buy Box winner ratio (0-1)
        /// </summary>
        public decimal BuyBoxRatio { get; set; }
    }
}
