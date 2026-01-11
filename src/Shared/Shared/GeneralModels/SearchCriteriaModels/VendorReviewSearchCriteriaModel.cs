using Common.Enumerations.Review;
using Common.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.GeneralModels.SearchCriteriaModels
{
	public class VendorReviewSearchCriteriaModel : BaseSearchCriteriaModel
	{
		/// <summary>
		/// Gets or sets the vendor identifier to filter reviews for a specific vendor.
		/// </summary>
		public Guid? VendorId { get; set; }

		/// <summary>
		/// Gets or sets the customer identifier to filter reviews by a specific customer.
		/// </summary>
		public Guid? CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the minimum rating value for filtering.
		/// Range: 1 to 5
		/// </summary>
		public int? RatingFrom { get; set; }

		/// <summary>
		/// Gets or sets the maximum rating value for filtering.
		/// Range: 1 to 5
		/// </summary>
		public int? RatingTo { get; set; }

		/// <summary>
		/// Gets or sets the list of review statuses to filter by.
		/// Multiple statuses can be specified (e.g., Pending, Approved, Rejected).
		/// </summary>
		public List<ReviewStatus>? Statuses { get; set; }

		/// <summary>
		/// Gets or sets the column name to sort by.
		/// Supported values: "Rating", "CreatedDateUtc"
		/// Default: "CreatedDateUtc"
		/// </summary>
		public string SortBy { get; set; } = "CreatedDateUtc";

		/// <summary>
		/// Gets or sets the sort direction.
		/// Supported values: "asc", "desc"
		/// Default: "desc"
		/// </summary>
		public string SortDirection { get; set; } = "desc";
	}
}
