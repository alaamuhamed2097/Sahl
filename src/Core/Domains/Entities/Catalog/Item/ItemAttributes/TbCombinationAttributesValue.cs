using Domains.Entities.Catalog.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
	public class TbCombinationAttributesValue : BaseEntity
	{
        [Required]
        public Guid ItemCombinationId { get; set; }

        [Required]
		public Guid AttributeId { get; set; }

		[StringLength(200)]
		public string Value { get; set; }

        // Navigation Properties
        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; }

		[ForeignKey("AttributeId")]
		public virtual TbAttribute Attribute { get; set; }
    }
}
