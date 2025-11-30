using Shared.DTOs.Base;

namespace Shared.DTOs.Review
{
    public class ReviewVoteDto : BaseDto
    {
        public Guid ReviewID { get; set; }
        public Guid CustomerID { get; set; }
        public string VoteType { get; set; } = null!;
        public string VoteValue { get; set; } = null!;
        public string WithType { get; set; } = null!;
    }
}
