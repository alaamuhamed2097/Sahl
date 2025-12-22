using Domains.Entities.ECommerceSystem;

namespace Domains.Entities.Location
{
    public class TbCity : BaseEntity
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;

        public Guid StateId { get; set; }
        public virtual TbState State { get; set; } = null!;
        public virtual IQueryable<TbCustomerAddress> CustomerAddresses { get; set; }
    }
}
