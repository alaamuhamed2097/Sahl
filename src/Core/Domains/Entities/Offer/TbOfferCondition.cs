using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domains.Entities.Offer
{
	public class TbOfferCondition : BaseEntity
	{
		[StringLength(100)]
		public string NameAr { get; set; }

		[StringLength(100)]
		public string NameEn { get; set; }

		public bool IsNew { get; set; }

		public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
	}
}
