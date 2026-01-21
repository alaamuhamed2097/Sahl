using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Filters
{
	public class CampaignSearchCriteriaModel : BaseSearchCriteriaModel
	{
		public Guid? CampaignId { get; set; }
		public int? Status { get; set; }
		public int? Type { get; set; }
		
	}
}
