namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Price range statistics
    /// </summary>
    public class PriceRangeDto
    {
        /// <summary>
        /// Minimum price in current results
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price in current results
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Average price in current results (optional)
        /// </summary>
        public decimal? AvgPrice { get; set; }
    }


}
