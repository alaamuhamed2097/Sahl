using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item.ItemAttributes;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Catalog.Attribute
{
    public class TbAttribute : BaseEntity
    {


        [StringLength(100)]
        public string TitleAr { get; set; }

        [StringLength(100)]
        public string TitleEn { get; set; }

        public bool IsRangeFieldType { get; set; }

        public int FieldType { get; set; }

        public int? MaxLength { get; set; }

        // Navigation Properties
        public virtual ICollection<TbCategoryAttribute> CategoryAttributes { get; set; }
        public virtual ICollection<TbAttributeOption> AttributeOptions { get; set; }
        public virtual ICollection<TbCombinationAttributesValue> CombinationAttributesValues { get; set; }
        public virtual ICollection<TbItemAttribute> ItemAttributes { get; set; }
    }
}
