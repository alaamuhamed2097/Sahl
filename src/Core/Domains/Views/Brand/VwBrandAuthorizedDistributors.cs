using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domains.Views.Brand
{
	public class VwBrandAuthorizedDistributors
	{

		public Guid BrandId { get; set; }
		public string BrandNameAr { get; set; } = string.Empty;
		public string BrandNameEn { get; set; } = string.Empty;

	
		public Guid DistributorId { get; set; }
		public string AuthorizationNumber { get; set; } = string.Empty;
		public DateTime AuthorizationStartDate { get; set; }
		public DateTime? AuthorizationEndDate { get; set; }
		public bool IsActive { get; set; }
		public string? AuthorizationDocumentPath { get; set; }
		public DateTime? VerifiedAt { get; set; }
		public string? Notes { get; set; }

		// Vendor
		public Guid VendorId { get; set; }
		public string? CompanyName { get; set; }
		public string? VendorCode { get; set; }
		public string? ContactName { get; set; }
		public decimal? VendorRating { get; set; }
		public string? VendorEmail { get; set; }
		public string? VendorPhone { get; set; }

		// Verifier
		public string? VerifierEmail { get; set; }
		public string? VerifierName { get; set; }

		// Status
		public string AuthorizationStatus { get; set; } = string.Empty;

		// Dates
		public DateTime CreatedDateUtc { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }
	}
}
