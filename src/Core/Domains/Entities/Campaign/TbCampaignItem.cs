using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
	[Index(nameof(CampaignId))]
	[Index(nameof(OfferCombinationPricingId))]
	[Index(nameof(VendorId))]
	[Index(nameof(CampaignId), nameof(OfferCombinationPricingId), IsUnique = true)]
	public class TbCampaignItem : BaseEntity
    {
        [Required]
        [ForeignKey("Campaign")]
        public Guid CampaignId { get; set; }
		[Required]
		[ForeignKey("OfferCombinationPricing")]
        public Guid OfferCombinationPricingId { get; set; }
		public Guid? VendorId { get; set; } 
        public bool IsActive { get; set; } = true;

        public virtual TbCampaign Campaign { get; set; } = null!;
        public virtual TbOfferCombinationPricing OfferCombinationPricing { get; set; } = null!;
	}
}
