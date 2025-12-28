using Common.Enumerations.Brand;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domains.Views.Brand
{
	public class VwBrandRegistrationRequests
	{
		
		public Guid RequestId { get; set; }
		public string BrandNameAr { get; set; } = string.Empty;
		public string BrandNameEn { get; set; } = string.Empty;
		public BrandType BrandType { get; set; }
		public BrandRegistrationStatus Status { get; set; }
		public string? DescriptionAr { get; set; }
		public string? DescriptionEn { get; set; }
		public string? OfficialWebsite { get; set; }
		public string? TrademarkNumber { get; set; }
		public DateTime? TrademarkExpiryDate { get; set; }
		public string? CommercialRegistrationNumber { get; set; }
		public DateTime? SubmittedAt { get; set; }
		public DateTime? ReviewedAt { get; set; }
		public string? ReviewNotes { get; set; }
		public string? RejectionReason { get; set; }

		// Vendor
		public Guid VendorId { get; set; }
		public string? CompanyName { get; set; }
		public string? VendorCode { get; set; }
		public string? ContactName { get; set; }
		public string? VendorEmail { get; set; }
		public string? VendorPhone { get; set; }

		// Reviewer
		public string? ReviewerEmail { get; set; }
		public string? ReviewerName { get; set; }

		// Approved Brand
		public Guid? ApprovedBrandId { get; set; }
		public string? ApprovedBrandNameAr { get; set; }
		public string? ApprovedBrandNameEn { get; set; }

		// Documents
		public int TotalDocuments { get; set; }
		public int VerifiedDocuments { get; set; }

		// Dates
		public DateTime CreatedDateUtc { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }
	
}
}
