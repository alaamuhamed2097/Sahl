using Domains.Entities.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Item
{
    public class TbItemAttribute : BaseEntity
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public Guid AttributeId { get; set; }

        public string Value { get; set; } = null!;

		[MaxLength(100)]
		public string TitleAr { get; set; } 

		[MaxLength(100)]
		public string TitleEn { get; set; } 

		public bool IsRangeFieldType { get; set; } 

		public int FieldType { get; set; }

		public int MaxLength { get; set; } 


		[ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;
        [ForeignKey("AttributeId")]
        public virtual TbAttribute Attribute { get; set; } = null!;
    }
}
