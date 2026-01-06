using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Review
{
	public class ResponseReviewVoteCountsDto
	{
		public Guid ReviewID { get; set; }
		public int HelpfulVotesCount { get; set; }
		public int NotHelpfulVotesCount { get; set; }

	}
}
