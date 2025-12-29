using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domains.Views.Brand
{
	public class VwBrandProducts
	{
		
		public Guid BrandId { get; set; }
		
		public Guid ItemId { get; set; }

		public string BrandNameAr { get; set; } = string.Empty;
		public string BrandNameEn { get; set; } = string.Empty;
		public string ItemTitleAr { get; set; } = string.Empty;
		public string ItemTitleEn { get; set; } = string.Empty;
		public string? SKU { get; set; }
		public string? Barcode { get; set; }
		public decimal? BasePrice { get; set; }
		public decimal? MinimumPrice { get; set; }
		public decimal? MaximumPrice { get; set; }
		public bool IsActive { get; set; }
		public decimal? ItemAverageRating { get; set; }
		public string ThumbnailImage { get; set; } = string.Empty;

		// Category
		public string CategoryNameAr { get; set; } = string.Empty;
		public string CategoryNameEn { get; set; } = string.Empty;

		// Unit
		public string UnitNameAr { get; set; } = string.Empty;
		public string UnitNameEn { get; set; } = string.Empty;

		// Statistics
		public int TotalOffers { get; set; }
		public int TotalCombinations { get; set; }
		public decimal? MinOfferPrice { get; set; }
		public decimal? MaxOfferPrice { get; set; }
		public int TotalAvailableStock { get; set; }

		// Reviews
		public int TotalReviews { get; set; }

		// Sales
		public int TotalOrderItems { get; set; }
		public int TotalQuantitySold { get; set; }
		public decimal TotalRevenue { get; set; }

		// Campaign
		public int ActiveCampaigns { get; set; }

		// Dates
		public DateTime CreatedDateUtc { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }
	}
}

