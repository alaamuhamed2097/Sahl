using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Brand
{
	public class VwBrandSalesAnalysisDto
	{
		// Brand
		public Guid BrandId { get; set; }
		public string BrandNameAr { get; set; } = string.Empty;
		public string BrandNameEn { get; set; } = string.Empty;

		// Time Period
		public int OrderYear { get; set; }
		public int OrderMonth { get; set; }
		public string MonthName { get; set; } = string.Empty;

		// Sales Metrics
		public int TotalOrders { get; set; }
		public int TotalOrderItems { get; set; }
		public int TotalQuantitySold { get; set; }
		public decimal TotalRevenue { get; set; }
		public decimal AverageUnitPrice { get; set; }

		// Product Metrics
		public int UniqueProductsSold { get; set; }
		public int UniqueVendors { get; set; }

		// Top Product
		public string? TopSellingProduct { get; set; }
	}
}
