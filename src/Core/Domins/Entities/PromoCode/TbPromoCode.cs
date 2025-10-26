using Common.Enumerations.PromoCode;
using Domains.Entities.Base;
using Domains.Entities.Order;

namespace Domains.Entities.PromoCode
{
    public class TbPromoCode : BaseEntity
    {
        public string TitleAR { get; set; } = null!;
        public string TitleEN { get; set; } = null!;
        public string Code { get; set; } = null!;
        public decimal Value { get; set; }

        public DateTime StartDateUTC { get; set; }
        public DateTime EndDateUTC { get; set; }
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }

        public PromoCodeType PromoCodeType { get; set; }
        public ICollection<TbOrder> Orders { get; set; } = new HashSet<TbOrder>();
    }
}
