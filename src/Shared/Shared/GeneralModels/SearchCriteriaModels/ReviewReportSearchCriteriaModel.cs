using Common.Enumerations.Review;
using Common.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.GeneralModels.SearchCriteriaModels
{
	public class ReviewReportSearchCriteriaModel : BaseSearchCriteriaModel
	{
		/// <summary>
		/// Review Id (optional)
		/// </summary>
		public Guid? ReviewId { get; set; }

		/// <summary>
		/// Reported Review Owner
		/// </summary>
		public Guid? CustomerId { get; set; }

		/// <summary>
		/// Reporter Customer
		/// </summary>
		public Guid? ReportedByCustomerId { get; set; }

		/// <summary>
		/// Report reason
		/// </summary>
		public ReviewReportReason? Reason { get; set; }

		/// <summary>
		/// Report status (Pending, Reviewed, Rejected)
		/// </summary>
		public ReviewReportStatus? Status { get; set; }

		/// <summary>
		/// From date
		/// </summary>
		public DateTime? FromDate { get; set; }

		/// <summary>
		/// To date
		/// </summary>
		public DateTime? ToDate { get; set; }
	}

}
