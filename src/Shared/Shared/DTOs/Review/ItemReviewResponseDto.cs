using Shared.DTOs.Base;

namespace Shared.DTOs.Review
{
	public class ItemReviewResponseDto : BaseDto
    {
		public required Guid ItemId { get; set; }
		public required string CustomerName { get; set; } = null!;
		public required string ReviewNumber { get; set; } = null!;
		public required string ReviewTitle { get; set; } = null!;
		public required string ReviewText { get; set; } = null!;
		public required decimal Rating { get; set; }
        public required int HelpfulVoteCount { get; set; }
		public required int CountReport { get; set; } = 0;
	}
}
