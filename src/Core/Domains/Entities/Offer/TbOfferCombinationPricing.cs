using Domains.Entities.Catalog.Item.ItemAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Offer
{
	public class TbOfferCombinationPricing : BaseEntity
	{
		[Required]
		public Guid ItemCombinationId { get; set; }

		[Required]
		public Guid OfferId { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal SalesPrice { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal? CostPrice { get; set; }

		public int AvailableQuantity { get; set; }

		public int ReservedQuantity { get; set; }

		public int RefundedQuantity { get; set; }

		public int DamagedQuantity { get; set; }

		public int InTransitQuantity { get; set; }

		public int ReturnedQuantity { get; set; }

		public int LockedQuantity { get; set; }

		public int StockStatus { get; set; }

		// Stock Management
		public int MinOrderQuantity { get; set; } = 1;
		public int MaxOrderQuantity { get; set; } = 999;
		public int LowStockThreshold { get; set; } = 5;

		// Seller specific SKU
		[StringLength(100)]
		public string? SellerSKU { get; set; }

		public bool IsActive { get; set; } = true;

		// Timestamps
		public DateTime? LastPriceUpdate { get; set; }
		public DateTime? LastStockUpdate { get; set; }

		[ForeignKey("ItemCombinationId")]
		public virtual TbItemCombination ItemCombination { get; set; }

		[ForeignKey("OfferId")]
		public virtual TbOffer Offer { get; set; }
	}
}
