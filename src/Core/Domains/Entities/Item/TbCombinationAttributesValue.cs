using Domains.Entities.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Item
{
	public class TbCombinationAttributesValue : BaseEntity
	{
		[Required]
		public Guid CombinationAttributeId { get; set; }

		[Required]
		public Guid AttributeId { get; set; }

		[StringLength(200)]
		public string Value { get; set; }

		// Navigation Properties
		[ForeignKey("CombinationAttributeId")]
		public virtual TbCombinationAttribute CombinationAttribute { get; set; }

		[ForeignKey("AttributeId")]
		public virtual TbAttribute Attribute { get; set; }
	}
}
