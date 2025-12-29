using Common.Enumerations.Pricing;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Contracts.Service.Catalog.Pricing;

/// <summary>
/// Pricing strategy interface
/// </summary>
public interface IPricingStrategy
{
    Task<PricingResult> CalculatePrice(
        Guid itemCombinationId,
        PricingStrategyType strategyType,
        int requestedQuantity);
    bool CanHandle(PricingStrategyType strategyType);
}
