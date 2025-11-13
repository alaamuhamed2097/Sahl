namespace Domains.Entities
{
    public class TbShippingCompany : BaseEntity
    {
        public string LogoImagePath { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<TbOrder> Orders { get; set; }
    }
}
