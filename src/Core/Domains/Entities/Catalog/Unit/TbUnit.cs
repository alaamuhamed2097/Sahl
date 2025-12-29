using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Catalog.Unit
{
    public class TbUnit : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;
        // Navigation Properties
        public ICollection<TbItem> Items { get; set; } = new List<TbItem>();
    }
}
