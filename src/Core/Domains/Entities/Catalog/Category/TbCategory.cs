using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Category
{
    public class TbCategory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleEn { get; set; } = null!;

        public Guid? ParentId { get; set; } = Guid.Empty;

        [Required]
        public bool IsFinal { get; set; }

        [Required]
        public bool IsHomeCategory { get; set; } // Indicates if the category will be displayed on the homepage or not.

        [Required]
		public bool IsRoot { get; set; }

        public bool IsFeaturedCategory { get; set; } // Indicates if category is featured on the homepage or not.
        [Required]
        public bool IsMainCategory { get; set; } // Indicates if the category is a main category.
        [Required]
        public bool PriceRequired { get; set; } // Indicates if the category required price or not.
        [Required]
        public int DisplayOrder { get; set; }
        [MaxLength(20)]
        public string TreeViewSerial { get; set; }
        [MaxLength(200)]
        public string? Icon { get; set; }
        [MaxLength(200)]
        public string? ImageUrl { get; set; }
        public virtual ICollection<TbCategoryAttribute> CategoryAttributes { get; set; } = new HashSet<TbCategoryAttribute>();
        public virtual ICollection<TbItem> Items { get; set; } = new HashSet<TbItem>();	
	}
}
