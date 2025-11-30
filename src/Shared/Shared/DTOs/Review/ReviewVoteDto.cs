using Shared.DTOs.Base;
using Common.Enumerations.Review;

namespace Shared.DTOs.Review
{
    public class ReviewVoteDto : BaseDto
    {
        public Guid ReviewID { get; set; }
        public Guid CustomerID { get; set; }
        public VoteType VoteType { get; set; }
    }
}
