using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.GeneralModels.SearchCriteriaModels
{
	public class OfferReviewSearchCriteriaModel : BaseSearchCriteriaModel
	{
		public Guid? OfferId { get; set; } = null;
		public Guid? CustomerId { get; set; } = null;
		public decimal? RatingFrom { get; set; } = null;
		public decimal? RatingTo { get; set; } = null;
		public bool? IsVerifiedPurchase { get; set; } = null;
		public List<Common.Enumerations.Review.ReviewStatus>? Statuses { get; set; } = null;

	}
}
