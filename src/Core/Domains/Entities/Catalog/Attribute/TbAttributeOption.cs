using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Attribute
{
    public class TbAttributeOption : BaseEntity
    {
        [MaxLength(100)]
        public string? TitleAr { get; set; }

        [MaxLength(100)]
        public string? TitleEn { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "DisplayOrder must be between 1 and 999.")]
        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [MaxLength(200)]
        public string? Value { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        [ForeignKey("AttributeId")]
        public virtual TbAttribute Attribute { get; set; } = null!;

		//----


	}
}
