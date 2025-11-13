using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domins.Entities.Item
{
    public class TbItemAttributeCombinationPricing : BaseEntity
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        [MaxLength(500)]
        public string AttributeIds { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;
    }
}
