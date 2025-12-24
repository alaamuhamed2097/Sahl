namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Available filter options for search interface
    /// </summary>
    public class AvailableFiltersDto
    {
        /// <summary>
        /// Available categories with item count
        /// </summary>
        public List<FilterOptionDto> Categories { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Available brands with item count
        /// </summary>
        public List<FilterOptionDto> Brands { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Price range statistics
        /// </summary>
        public PriceRangeDto PriceRange { get; set; }
    }
}
