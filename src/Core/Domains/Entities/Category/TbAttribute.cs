using Common.Enumerations.FieldType;
using Domains.Entities.Item;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Category
{
    public class TbAttribute : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        public bool IsRangeFieldType { get; set; } = false;

        [Required]
        public FieldType FieldType { get; set; } // 1 = String, 2 = Integer, etc.

        public int MaxLength { get; set; }

        public virtual ICollection<TbCategoryAttribute> CategoryAttributes { get; set; }

        public virtual ICollection<TbAttributeOption> AttributeOptions { get; set; }

        public virtual ICollection<TbItemAttribute> ItemAttributes { get; set; }
    }
}
