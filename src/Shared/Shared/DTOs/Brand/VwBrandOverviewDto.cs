using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Brand
{
	public class VwBrandOverviewDto
	{
		// Brand Info
		public Guid BrandId { get; set; }
		public string NameAr { get; set; } = string.Empty;
		public string NameEn { get; set; } = string.Empty;
		public string TitleAr { get; set; } = string.Empty;
		public string TitleEn { get; set; } = string.Empty;
		public string DescriptionAr { get; set; } = string.Empty;
		public string DescriptionEn { get; set; } = string.Empty;
		public string LogoPath { get; set; } = string.Empty;
		public string? WebsiteUrl { get; set; }
		public bool IsPopular { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedDateUtc { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }

		// Statistics
		public int TotalProducts { get; set; }
		public int ActiveProducts { get; set; }
		public int TotalOffers { get; set; }
		public int TotalVendors { get; set; }

		// Reviews
		public decimal? AverageRating { get; set; }
		public int TotalReviews { get; set; }

		// Sales
		public int TotalOrders { get; set; }
		public decimal TotalSales { get; set; }

		// Registration Requests
		public int TotalRegistrationRequests { get; set; }
		public int ApprovedRequests { get; set; }
		public int PendingRequests { get; set; }

		// Authorized Distributors
		public int TotalAuthorizedDistributors { get; set; }
		public int ActiveDistributors { get; set; }
	}
}
