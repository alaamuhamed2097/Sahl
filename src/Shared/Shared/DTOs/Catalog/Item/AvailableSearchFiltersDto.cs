namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Available filter options for search interface
    /// </summary>
    public class AvailableSearchFiltersDto
    {
        /// <summary>
        /// Available categories with item counts
        /// </summary>
        public List<FilterOptionDto> Categories { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Available brands with item counts
        /// </summary>
        public List<FilterOptionDto> Brands { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Available vendors with item counts
        /// </summary>
        public List<FilterOptionDto> Vendors { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Available attributes with item counts
        /// </summary>
        public List<AttributeFilterDto> Attributes { get; set; } = new List<AttributeFilterDto>();

        /// <summary>
        /// Available conditions with item counts
        /// </summary>
        public List<FilterOptionDto> Conditions { get; set; } = new List<FilterOptionDto>();

        /// <summary>
        /// Price range statistics
        /// </summary>
        public PriceRangeDto PriceRange { get; set; }
    }
}
