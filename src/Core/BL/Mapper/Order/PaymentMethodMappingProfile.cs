using Domains.Entities.Order.Payment;
using Shared.DTOs.Order.Payment;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigurePaymentMethodMappings()
    {
        CreateMap<TbPaymentMethod, PaymentMethodDto>().ReverseMap();
    }
}
