using Domains.Entities.Catalog.Item;
using Domains.Entities.Merchandising.HomePage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Merchandising.HomePageBlocks
{
    public class TbBlockItem : BaseEntity
    {
        [Required]
        [ForeignKey("HomepageBlock")]
        public Guid HomepageBlockId { get; set; }

        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        public int DisplayOrder { get; set; }

        public virtual TbHomepageBlock HomepageBlock { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
    }
}
