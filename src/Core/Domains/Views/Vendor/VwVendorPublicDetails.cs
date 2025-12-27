using Common.Enumerations.VendorType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Views.Vendor
{
	public class VwVendorPublicDetails
	{
		public Guid VendorId { get; set; }
		public string? CompanyName { get; set; }
		public string? ContactName { get; set; }
		public VendorType VendorType { get; set; }
		public bool IsActive { get; set; }
		public decimal? Rating { get; set; }

		// Reviews KPIs
		public int TotalReviews { get; set; }
		public decimal? AverageRating { get; set; }
	}
}
