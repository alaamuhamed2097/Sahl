using Common.Filters;

namespace Shared.GeneralModels.SearchCriteriaModels
{
    public class ItemSearchCriteriaModel : BaseSearchCriteriaModel
    {
        // Categories
        public List<Guid>? CategoryIds { get; set; } = null;

        // Stock Status
        public bool? IsInStock { get; set; } = null;

        // Range for price
        public decimal? PriceFrom { get; set; } = null;
        public decimal? PriceTo { get; set; } = null;

        // Range for Quantity
        public decimal? QuantityFrom { get; set; } = null;
        public decimal? QuantityTo { get; set; } = null;

        // New Item Flags Filters
        public bool? IsNewArrival { get; set; } = null;
    }
}
