using Domains.Entities.Base;
using Domains.Entities.Items;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Brand
{
    public class TbBrand : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string NameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NameAr { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TitleEn { get; set; }

        [StringLength(100)]
        public string? TitleAr { get; set; }

        [StringLength(200)]
        public string? DescriptionEn { get; set; }

        [StringLength(200)]
        public string? DescriptionAr { get; set; }

        [Required]
        [StringLength(200)]
        public string LogoPath { get; set; } = string.Empty;

        [StringLength(200)]
        public string? WebsiteUrl { get; set; }

        public bool IsFavorite { get; set; } = false;

        public int DisplayOrder { get; set; } = 0;

        // Navigation property for Items
        public virtual ICollection<TbItem> Items { get; set; } = new List<TbItem>();
    }
}