using Common.Enumerations.Offer;

namespace Shared.GeneralModels.SearchCriteriaModels
{
    public class ItemstatusSearchCriteriaModel : BaseSearchCriteriaModel
    {
		public List<StockStatus>? StockStatuses { get; set; }
		public List<OfferVisibilityScope>? VisibilityScopes { get; set; }
		public bool? IsNewArrival { get; set; }
		public List<Guid>? CategoryIds { get; set; }

	}
}
