using Domains.Entities.Catalog.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Merchandising.HomePage
{
    public class TbBlockCategory : BaseEntity
    {
        [Required]
        [ForeignKey("HomepageBlock")]
        public Guid HomepageBlockId { get; set; }

        [Required]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        // Relations
        public virtual TbHomepageBlock HomepageBlock { get; set; } = null!;
        public virtual TbCategory Category { get; set; } = null!;
    }
}