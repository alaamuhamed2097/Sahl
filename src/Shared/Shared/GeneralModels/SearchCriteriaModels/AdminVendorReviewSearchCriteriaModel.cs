using Common.Enumerations.Review;
using Common.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.GeneralModels.SearchCriteriaModels
{
	public class AdminVendorReviewSearchCriteriaModel : VendorReviewSearchCriteriaModel
	{
		/// <summary>
		/// Gets or sets the vendor identifier to filter reviews for a specific vendor.
		/// </summary>
		public Guid? VendorId { get; set; }
	}
}
