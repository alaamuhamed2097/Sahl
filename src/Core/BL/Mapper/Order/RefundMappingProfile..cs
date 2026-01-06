using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Order;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Shipping;
using Shared.DTOs.ECommerce;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.Payment.Refund;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureRefundMappings()
    {
        CreateMap<TbRefund, RefundDto>().ReverseMap();
        CreateMap<TbRefund, RefundRequestDto>().ReverseMap();
    }
}
