using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.Offer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
	public class TbItemCombination : BaseEntity
	{
		[Required]
		public Guid ItemId { get; set; }
      
        [StringLength(200)]
        public string Barcode { get; set; }

        [StringLength(200)]
        public string SKU { get; set; }
       
        public bool IsDefault { get; set; } = false;

        // Navigation Properties
        [ForeignKey("ItemId")]
		public virtual TbItem Item { get; set; }

        public virtual ICollection<TbCombinationAttribute> CombinationAttributes { get; set; }
        public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
		public virtual ICollection<TbItemCombinationImage> ItemCombinationImages { get; set; }
		public virtual ICollection<TbCustomerItemView> CustomerItemViews { get; set; }
	}
}
