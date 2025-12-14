namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Price range statistics
    /// </summary>
    public class PriceRangeDto
    {
        /// <summary>
        /// Minimum price in search results
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price in search results
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Average price in search results
        /// </summary>
        public decimal AvgPrice { get; set; }
    }
}
