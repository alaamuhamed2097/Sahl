using Domains.Entities.CouponCode;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.Order;
using Domains.Entities.Shipping;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.CouponCode;
using Shared.DTOs.ECommerce.Order;
using Shared.DTOs.Review;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureProductReviewMappings()
        {
            CreateMap<TbProductReview, ProductReviewDto>().ReverseMap();
        }
    }
}
