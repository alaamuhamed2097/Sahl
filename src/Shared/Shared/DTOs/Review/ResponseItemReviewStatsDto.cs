using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Review
{
	public class ResponseItemReviewStatsDto
	{
		public decimal AverageRating { get; set; }
		public int ReviewCount { get; set; }
		public int FiveStarCount { get; set; }
		public int FourStarCount { get; set; }
		public int ThreeStarCount { get; set; }
		public int TwoStarCount { get; set; }
		public int OneStarCount { get; set; }
		public decimal FiveStarPercentage { get; set; }
		public decimal FourStarPercentage { get; set; }
		public decimal ThreeStarPercentage { get; set; }
		public decimal TwoStarPercentage { get; set; }
		public decimal OneStarPercentage { get; set; }
	}
}
