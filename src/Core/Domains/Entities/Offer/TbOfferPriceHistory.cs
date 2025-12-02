using Domains.Entities.Catalog.Item.ItemAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Offer
{
    public class TbOfferPriceHistory : BaseEntity
	{
		[Required]
		public Guid ItemCombinationId { get; set; }

		[Required]
		public Guid OfferCombinationPricingId { get; set; }


		[Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldPrice { get; set; }

		[Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NewPrice { get; set; }

		[StringLength(500)]
		public string? ChangeNote { get; set; }

        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; }

        [ForeignKey("OfferCombinationPricingId")]
        public virtual TbOfferCombinationPricing OfferCombinationPricing { get; set; }
    }
     
}
