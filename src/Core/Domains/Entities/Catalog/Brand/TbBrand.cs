using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Catalog.Brand
{
    public class TbBrand : BaseEntity
    {
        [StringLength(50)]
        public string NameAr { get; set; }

        [StringLength(50)]
        public string NameEn { get; set; }

        [StringLength(100)]
        public string DescriptionAr { get; set; }

        [StringLength(200)]
        public string DescriptionEn { get; set; }

        [StringLength(200)]
        public string LogoPath { get; set; }

        [StringLength(200)]
        public string WebsiteUrl { get; set; }

        public int DisplayOrder { get; set; }

        // Navigation Properties
        public virtual ICollection<TbItem> Items { get; set; }
    }
}