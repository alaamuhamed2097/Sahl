using Common.Enumerations.Offer.Warranty;
using Domains.Entities.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Offer.Warranty
{
	public class TbWarranty : BaseEntity
	{
		public WarrantyType WarrantyType { get; set; }

		public int WarrantyPeriodMonths { get; set; }

		[StringLength(500)]
		public string WarrantyPolicy { get; set; }

		[StringLength(500)]
		public string WarrantyServiceCenter { get; set; }

		public virtual ICollection<TbOffer> OffersList { get; set; } = new HashSet<TbOffer>();
	}
}
