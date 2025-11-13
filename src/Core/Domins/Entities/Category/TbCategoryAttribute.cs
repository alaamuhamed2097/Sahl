using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domins.Entities.Category
{
    public class TbCategoryAttribute : BaseEntity
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool AffectsPricing { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        public int DisplayOrder { get; set; }

        [ForeignKey("CategoryId")]
        public virtual TbCategory Category { get; set; }

        [ForeignKey("AttributeId")]
        public virtual TbAttribute Attribute { get; set; }
    }
}
