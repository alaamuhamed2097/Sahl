using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domins.Entities.Unit
{
    public class TbUnitConversion : BaseEntity
    {
        [Required]
        public Guid FromUnitId { get; set; }

        [Required]
        public Guid ToUnitId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal ConversionFactor { get; set; }

        [ForeignKey("FromUnitId")]
        public virtual TbUnit FromUnit { get; set; } = null!;

        [ForeignKey("ToUnitId")]
        public virtual TbUnit ToUnit { get; set; } = null!;
    }
}
