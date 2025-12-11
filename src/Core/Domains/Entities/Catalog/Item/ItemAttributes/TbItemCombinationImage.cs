using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Item.ItemAttributes
{
    public class TbItemCombinationImage : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Path { get; set; } = null!;

        [Required]
        public int Order { get; set; }

        [Required]
        public Guid ItemCombinationId { get; set; }

        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; } = null!;
	}
}
