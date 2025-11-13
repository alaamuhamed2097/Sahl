using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Category
{
    public class TbAttributeOption : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        [Required]
        [Range(1, 999, ErrorMessage = "DisplayOrder must be between 1 and 999.")]
        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        [ForeignKey("AttributeId")]
        public virtual TbAttribute Attribute { get; set; } = null!;
    }
}
