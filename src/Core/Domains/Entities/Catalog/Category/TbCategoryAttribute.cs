using Domains.Entities.Catalog.Attribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Category
{
    public class TbCategoryAttribute : BaseEntity
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        /// <summary>
        /// If true this attribute affects pricing for items in this category
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool AffectsPricing { get; set; }

        /// <summary>
        /// If true this attribute is used to generate variants (combinations / SKUs) for items in this category
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool IsVariant { get; set; }

        /// <summary>
        /// If true this attribute affects stock (each value may produce separate inventory)
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool AffectsStock { get; set; }

        /// <summary>
        /// If true the attribute is required for the item/category
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Display / sort order for admin UI
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Additional explicit sort order property (kept for clarity)
        /// </summary>
        public int SortOrder { get; set; }

        [ForeignKey("CategoryId")]
        public virtual TbCategory Category { get; set; }

        [ForeignKey("AttributeId")]
        public virtual TbAttribute Attribute { get; set; }
    }
}
