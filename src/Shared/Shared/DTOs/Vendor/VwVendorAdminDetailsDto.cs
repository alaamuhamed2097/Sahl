using Common.Enumerations.VendorType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Vendor
{
	public class VwVendorAdminDetailsDto
	{
		public Guid VendorId { get; set; }
		public string UserId { get; set; } = null!;
		public string? CompanyName { get; set; }
		public string? ContactName { get; set; }
		public VendorType VendorType { get; set; }
		public string? Address { get; set; }
		public string? PostalCode { get; set; }
		public bool IsActive { get; set; }

		// Legal & Financial
		public string? CommercialRegister { get; set; }
		public bool VATRegistered { get; set; }
		public string? TaxNumber { get; set; }
		public string? Notes { get; set; }

		// Reviews KPIs
		public int TotalReviews { get; set; }
		public decimal? AverageRating { get; set; }
		public int VerifiedReviewsCount { get; set; }
	}
}
