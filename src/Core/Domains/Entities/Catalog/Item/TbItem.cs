using Domains.Entities.Catalog.Brand;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Catalog.Unit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Item
{
    public class TbItem : BaseSeo
    {

        [StringLength(100)]
        public string TitleAr { get; set; }

        [StringLength(100)]
        public string TitleEn { get; set; }

        [StringLength(200)]
        public string ShortDescriptionAr { get; set; }

        [StringLength(200)]
        public string ShortDescriptionEn { get; set; }

        [StringLength(200)]
        public string Barcode { get; set; }

        [StringLength(200)]
        public string SKU { get; set; }

        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Guid? UnitId { get; set; }

        public Guid? VideoProviderId { get; set; }
        public Guid? ByBoxOfferId { get; set; }

        [StringLength(200)]
        public string VideoUrl { get; set; }

        [StringLength(200)]
        public string ThumbnailImage { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaximumPrice { get; set; }

        public bool IsNewArrival { get; set; }

        public Guid? BrandId { get; set; }

        public int VisibilityScope { get; set; }

        // Navigation Properties
        [ForeignKey("CategoryId")]
        public virtual TbCategory Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual TbBrand Brand { get; set; }

        [ForeignKey("UnitId")]
        public virtual TbUnit Unit { get; set; }

        public virtual ICollection<TbItemImage> ItemImages { get; set; }
        public virtual ICollection<TbItemCombination> ItemCombinations { get; set; }
    }
}
