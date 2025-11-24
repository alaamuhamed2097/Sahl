using Domains.Entities.Catalog.Item;
using Domains.Entities.Shipping;
using Domains.Entities.Warranty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.OffersPricing
{
	public class TbOffer : BaseEntity
	{
		[Required]
		public Guid ItemId { get; set; }

		[Required]
		public Guid UserId { get; set; }

		public Guid? OfferConditionId { get; set; }

		public Guid? WarrantyId { get; set; }

		public int StorageLocation { get; set; }

		public int HandlingTimeDays { get; set; }

		public bool IsProtectingCorrections { get; set; }

		public int VisibilityScope { get; set; }

		[ForeignKey("ItemId")]
		public virtual TbItem Item { get; set; }

		[ForeignKey("UserId")]
		public virtual AspNetUser User { get; set; }

		[ForeignKey("OfferConditionId")]
		public virtual TbOfferCondition OfferCondition { get; set; }

		[ForeignKey("WarrantyId")]
		public virtual TbWarranty Warranty { get; set; }

		public virtual ICollection<TbUserOfferRating> UserOfferRatings { get; set; }
		public virtual ICollection<TbShippingDetail> ShippingDetails { get; set; }
		public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
	}
}
