using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Contracts.Service.Catalog.Pricing;

/// <summary>
/// Main pricing service that uses the appropriate strategy
/// </summary>
public interface IPricingService
{
    Task<PricingResult> CalculatePrice(Guid itemCombinationId,
 PricingStrategyType strategyType,
 int requestedQuantity);
}
