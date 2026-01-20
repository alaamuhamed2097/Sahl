 using Common.Enumerations.Offer;
using Domains.Entities.BuyBox;
using Domains.Entities.Campaign;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Catalog.Pricing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Offer
{
    public class TbOfferCombinationPricing : BaseEntity
    {
        [Required]
        public Guid ItemCombinationId { get; set; }

        [Required]
        public Guid OfferId { get; set; }

		[Required]
        public Guid OfferConditionId { get; set; }
        
        [StringLength(200)]
        public string Barcode { get; set; } = null!;

        [StringLength(200)]
        public string SKU { get; set; } = null!;

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

		public StockStatus StockStatus { get; set; }
		public DateTime? LastStockUpdate { get; set; }

        // Stock Management
        public int MinOrderQuantity { get; set; } = 1;
        public int MaxOrderQuantity { get; set; } = 999;
        public int LowStockThreshold { get; set; } = 5;

        // Vendor performance metrics
        public bool IsBuyBoxWinner { get; set; } = false;

        [ForeignKey("OfferConditionId")]
        public virtual TbOfferCondition OfferCondition { get; set; }

        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; }

        [ForeignKey("OfferId")]
        public virtual TbOffer Offer { get; set; }

        public virtual ICollection<TbOfferPriceHistory> OfferPriceHistories { get; set; }
        public virtual ICollection<TbQuantityTierPricing> QuantityTierPricings { get; set; }
        public virtual ICollection<TbBuyBoxCalculation> BuyBoxCalculations { get; set; }
		public virtual ICollection<TbCampaignItem> CampaignItems { get; set; } 

	}
}
