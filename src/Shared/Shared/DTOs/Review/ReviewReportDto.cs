using Common.Enumerations.Review;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Review
{
	public class ReviewReportDto : BaseDto
	{
		//[Required]
		//public Guid ReportID { get; set; }

		[Required]
		public Guid ItemReviewId { get; set; }

		[Required]
		public Guid CustomerId { get; set; }

		[Required]
		[EnumDataType(typeof(ReviewReportReason))]
		public ReviewReportReason Reason { get; set; }

		[MaxLength(1000)]
		public string? Details { get; set; }

		[Required]
		[EnumDataType(typeof(ReviewReportStatus))]
		public ReviewReportStatus Status { get; set; }
	}

}
