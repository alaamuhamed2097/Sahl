namespace Domains.Entities.Items
{
    using Domains.Entities.Base;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TbAttribute : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        [Required]
        public int FieldType { get; set; } // 1 = String, 2 = Integer, etc.

        public int MaxLength { get; set; }

        public virtual ICollection<TbCategoryAttribute> CategoryAttributes { get; set; }
        public virtual ICollection<TbAttributeOption> AttributeOptions { get; set; }
        public virtual ICollection<TbItemAttribute> ItemAttributes { get; set; }
    }
}
