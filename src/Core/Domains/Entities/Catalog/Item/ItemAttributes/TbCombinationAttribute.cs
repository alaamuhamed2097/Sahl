using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
    public class TbCombinationAttribute : BaseEntity
	{
        [Required]
        public Guid ItemCombinationId { get; set; }
        [Required]
        public Guid AttributeValueId { get; set; }

        // Navigation Properties
        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; }
        [ForeignKey("AttributeValueId")]
        public virtual TbCombinationAttributesValue CombinationAttributeValue { get; set; }
    }
}
