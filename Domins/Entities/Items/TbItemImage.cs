using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Items
{
    public class TbItemImage : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Path { get; set; } = null!;

        [Required]
        public int Order { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;
    }
}
