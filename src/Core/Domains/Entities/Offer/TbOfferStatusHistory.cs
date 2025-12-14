using Domains.Entities.Catalog.Item.ItemAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Offer
{
    public class TbOfferStatusHistory : BaseEntity
	{
		[Required]
		public Guid OfferId { get; set; }

		[Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldStatus { get; set; }

		[Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NewStatus { get; set; }

		[StringLength(500)]
		public string? ChangeNote { get; set; }

        [ForeignKey("OfferId")]
        public virtual TbOffer Offer { get; set; }
    }
}
