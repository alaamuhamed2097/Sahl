using Domains.Entities.ECommerceSystem.Review;

namespace Domains.Entities.Shipping
{
    public class TbShippingCompany : BaseEntity
    {
        public string LogoImagePath { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<TbOrder> Orders { get; set; } 
		public virtual ICollection<TbShippingCompanyReview> ShippingCompanyReviews { get; set; } = new List<TbShippingCompanyReview>();

	}
}
