using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Review
{
    public class ItemReviewDto : BaseDto
    {
		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public Guid ItemID { get; set; }

		[Range(0, 5, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		public decimal Rating { get; set; }

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(100, MinimumLength = 2, ErrorMessageResourceName = "Length", ErrorMessageResourceType = typeof(ValidationResources))]
		public string ReviewTitle { get; set; } = null!;

		[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
		[StringLength(300, MinimumLength = 2, ErrorMessageResourceName = "Length", ErrorMessageResourceType = typeof(ValidationResources))]
		public string ReviewText { get; set; } = null!;
	}
}
