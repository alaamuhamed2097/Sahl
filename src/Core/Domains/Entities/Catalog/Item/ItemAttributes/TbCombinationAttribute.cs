using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
	public class TbCombinationAttribute : BaseEntity
	{
		[Required]
		public Guid ItemCombinationId { get; set; }

		// Navigation Properties
		[ForeignKey("ItemCombinationId")]
		public virtual TbItemCombination ItemCombination { get; set; }

		// NOTE: The design keeps a minimal linking table.
		// The actual attribute values are stored in TbCombinationAttributesValue which
		// references this entity via CombinationAttributeId. Keeping this entity allows
		// future extension (ordering, group metadata) without heavy schema changes.
	}
}
