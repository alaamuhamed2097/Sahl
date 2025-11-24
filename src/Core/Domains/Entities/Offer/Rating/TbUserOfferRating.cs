using Domains.Entities.Offer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Offer.Rating
{
	public class TbUserOfferRating : BaseEntity
	{
		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid OfferId { get; set; }

		public int Rating { get; set; }

		[StringLength(500)]
		public string Comment { get; set; }

		public bool IsVerifiedPurchase { get; set; }

		[ForeignKey("OfferId")]
		public virtual TbOffer Offer { get; set; }

		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}
}
