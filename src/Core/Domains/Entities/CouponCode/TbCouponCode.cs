using Common.Enumerations;

namespace Domains.Entities.CouponCode
{
    public class TbCouponCode : BaseEntity
    {
        public string TitleAR { get; set; } = null!;
        public string TitleEN { get; set; } = null!;
        public string Code { get; set; } = null!;
        public decimal Value { get; set; }

        public DateTime StartDateUTC { get; set; }
        public DateTime EndDateUTC { get; set; }
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }

        public CouponCodeType CouponCodeType { get; set; }
        public ICollection<TbOrder> Orders { get; set; } = new HashSet<TbOrder>();
    }
}
