using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Item
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

        /// <summary>
        /// Indicates if this is the default pricing combination for the item.
        /// Only one combination per item can be marked as default.
        /// </summary>
        [Required]
        public bool IsDefault { get; set; } = false;

        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;
    }
}
