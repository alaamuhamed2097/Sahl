using Domains.Entities;
using Domins.Entities.Brand;
using Domins.Entities.Category;
using Domins.Entities.Unit;
using Domins.Entities.VideoProvider;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domins.Entities.Item
{
    public class TbItem : BaseSeo
    {
        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string TitleEn { get; set; } = null!;

        [MaxLength(500)]
        public string ShortDescriptionAr { get; set; } = null!;

        [MaxLength(500)]
        public string ShortDescriptionEn { get; set; } = null!;

        [MaxLength(50000)]
        public string DescriptionAr { get; set; } = null!;

        [MaxLength(50000)]
        public string DescriptionEn { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid UnitId { get; set; }

        public Guid BrandId { get; set; }

        public Guid? VideoProviderId { get; set; }

        [MaxLength(200)]
        public string? VideoLink { get; set; }

        [MaxLength(200)]
        public string ThumbnailImage { get; set; } = null!;

        [Required]
        [DefaultValue(false)]
        public bool StockStatus { get; set; }

        public int? Quantity { get; set; }

        public decimal Price { get; set; }

        // New Item Flags
        [DefaultValue(false)]
        public bool IsNewArrival { get; set; } = false;

        [DefaultValue(false)]
        public bool IsBestSeller { get; set; } = false;

        [DefaultValue(false)]
        public bool IsRecommended { get; set; } = false;

        [ForeignKey("CategoryId")]
        public virtual TbCategory Category { get; set; } = null!;

        [ForeignKey("UnitId")]
        public virtual TbUnit Unit { get; set; } = null!;

        [ForeignKey("BrandId")]
        public virtual TbBrand Brand { get; set; }

        [ForeignKey("VideoProviderId")]
        public virtual TbVideoProvider VideoProvider { get; set; } = null!;
        //public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; } = null!;

        public virtual ICollection<TbItemImage> ItemImages { get; set; } = null!;
        public virtual ICollection<TbItemAttribute> ItemAttributes { get; set; } = null!;
        public virtual ICollection<TbItemAttributeCombinationPricing> ItemAttributeCombinationPricings { get; set; } = null!;
    }
}
