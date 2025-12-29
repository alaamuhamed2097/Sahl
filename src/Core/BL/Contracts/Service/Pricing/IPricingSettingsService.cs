using Shared.DTOs.Pricing;

namespace BL.Contracts.Service.Pricing;

public interface IPricingSettingsService
{
    Task<IEnumerable<PricingSystemSettingDto>> GetAllSystemsAsync();
}
