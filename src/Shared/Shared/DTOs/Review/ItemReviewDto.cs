using Common.Enumerations.Review;
using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Review
{
    public class ItemReviewDto : BaseDto
    {
		[Required]
		public int ReviewNumber { get; set; }

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public Guid ItemID { get; set; }

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public Guid CustomerID { get; set; }

		//public Guid? OrderItemID { get; set; }

		[Range(0, 5, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public decimal Rating { get; set; }

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleLength", ErrorMessageResourceType = typeof(ValidationResources))]
		public string ReviewTitle { get; set; } = null!;

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(1000, MinimumLength = 2, ErrorMessageResourceName = "ReviewTextLength", ErrorMessageResourceType = typeof(ValidationResources))]
		public string ReviewText { get; set; } = null!;

		//public bool IsVerifiedPurchase { get; set; } = false;

		[Range(0, int.MaxValue, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public int HelpfulCount { get; set; } = 0;

		[Range(0, int.MaxValue, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public int NotHelpfulCount { get; set; } = 0;

		[Required]
		public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

		public bool IsEdited { get; set; } = false;
	}
}
