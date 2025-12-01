using Shared.DTOs.Pricing;

namespace BL.Contracts.Service.Pricing
{
    public interface IPricingService
    {
        Task<IEnumerable<PricingSystemSettingDto>> GetAllSystemsAsync();
    }
}
