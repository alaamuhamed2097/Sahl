using Common.Enumerations.Review;
using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.Review
{
	public class VendorReviewDto : BaseDto
	{
		

		// FK
		[Required(ErrorMessage = "CustomerId is required")]
		public Guid CustomerId { get; set; }

		[Required(ErrorMessage = "VendorId is required")]
		public Guid VendorId { get; set; }

		public Guid? OrderDetailId { get; set; }
		// Review Data
		[Required(ErrorMessage = "Rating is required")]
		[Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
		public decimal Rating { get; set; }

		[MaxLength(1000, ErrorMessage = "Review text cannot exceed 1000 characters")]
		public string? ReviewText { get; set; }

		// Status & Flags
		public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

		public bool IsEdited { get; set; }

		public bool IsVerifiedPurchase { get; set; }

	}

}
