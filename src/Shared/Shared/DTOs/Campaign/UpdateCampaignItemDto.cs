using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs.Campaign
{
	public class UpdateCampaignItemDto
	{
		[Required]
		public Guid CampaignId { get; set; }

		[Required]
		public Guid OfferCombinationPricingId { get; set; }

		public int? MaxQuantityPerOrder { get; set; }

		public int? TotalAvailableQuantity { get; set; }

		public bool IsActive { get; set; }
	}
}
