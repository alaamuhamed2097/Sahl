using Common.Enumerations.Review;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.Review
{
	public class ResponseItemReviewDto
	{
		public Guid Id { get; set; }
		public string ReviewNumber { get; set; }

		public Guid ItemID { get; set; }

		public Guid CustomerID { get; set; }

		//public Guid? OrderItemID { get; set; }

		public decimal Rating { get; set; }

		public string ReviewTitle { get; set; } = null!;

		public string ReviewText { get; set; } = null!;

		//public bool IsVerifiedPurchase { get; set; } = false;

		

	}
}
