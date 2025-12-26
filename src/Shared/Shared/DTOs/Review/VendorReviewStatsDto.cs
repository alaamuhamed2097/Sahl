using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Review
{
	/// <summary>
	/// Data Transfer Object for Vendor review statistics.
	/// Contains aggregated metrics including average rating, total review count, and rating distribution.
	/// </summary>
	public class VendorReviewStatsDto
	{
		/// <summary>
		/// Gets or sets the vendor identifier.
		/// </summary>
		public Guid VendorId { get; set; }

		/// <summary>
		/// Gets or sets the average rating across all approved reviews.
		/// Range: 0.0 to 5.0
		/// </summary>
		public decimal AverageRating { get; set; }

		/// <summary>
		/// Gets or sets the total count of approved reviews.
		/// </summary>
		public int ReviewCount { get; set; }

		/// <summary>
		/// Gets or sets the count of 5-star reviews.
		/// </summary>
		public int FiveStarCount { get; set; }

		/// <summary>
		/// Gets or sets the count of 4-star reviews.
		/// </summary>
		public int FourStarCount { get; set; }

		/// <summary>
		/// Gets or sets the count of 3-star reviews.
		/// </summary>
		public int ThreeStarCount { get; set; }

		/// <summary>
		/// Gets or sets the count of 2-star reviews.
		/// </summary>
		public int TwoStarCount { get; set; }

		/// <summary>
		/// Gets or sets the count of 1-star reviews.
		/// </summary>
		public int OneStarCount { get; set; }

		/// <summary>
		/// Gets or sets the percentage of 5-star reviews.
		/// Range: 0.0 to 100.0
		/// </summary>
		public decimal FiveStarPercentage { get; set; }

		/// <summary>
		/// Gets or sets the percentage of 4-star reviews.
		/// Range: 0.0 to 100.0
		/// </summary>
		public decimal FourStarPercentage { get; set; }

		/// <summary>
		/// Gets or sets the percentage of 3-star reviews.
		/// Range: 0.0 to 100.0
		/// </summary>
		public decimal ThreeStarPercentage { get; set; }

		/// <summary>
		/// Gets or sets the percentage of 2-star reviews.
		/// Range: 0.0 to 100.0
		/// </summary>
		public decimal TwoStarPercentage { get; set; }

		/// <summary>
		/// Gets or sets the percentage of 1-star reviews.
		/// Range: 0.0 to 100.0
		/// </summary>
		public decimal OneStarPercentage { get; set; }
	}
}
