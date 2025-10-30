using Domains.Entities.Base;
using System.ComponentModel;
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
        public decimal FinalPrice { get; set; }

        [DefaultValue(0)]
        public int Quantity { get; set; }

        [MaxLength(255)]
        public string? Image { get; set; }

        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;
    }
}
