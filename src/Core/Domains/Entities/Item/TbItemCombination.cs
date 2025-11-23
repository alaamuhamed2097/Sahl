using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.Item
{
	public class TbItemCombination : BaseEntity
	{
		[Required]
		public Guid ItemId { get; set; }

		// Navigation Properties
		[ForeignKey("ItemId")]
		public virtual TbItem Item { get; set; }

		public virtual ICollection<TbCombinationAttribute> CombinationAttributes { get; set; }
	}
}
