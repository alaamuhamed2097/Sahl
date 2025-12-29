using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using DAL.Contracts.Repositories;
using Domains.Entities.Offer;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Services.Catalog.Pricing;

public class SimplePricingStrategy : IPricingStrategy
{
    private readonly ITableRepository<TbOfferCombinationPricing> _combinationPricingRepository;
    public SimplePricingStrategy(ITableRepository<TbOfferCombinationPricing> combinationPricingRepository)
    {
        _combinationPricingRepository = combinationPricingRepository;
    }

    public bool CanHandle(PricingStrategyType strategyType)
    {
        return strategyType == PricingStrategyType.Simple;
    }

    public async Task<PricingResult> CalculatePrice(
        Guid itemCombinationId,
        PricingStrategyType strategyType,
        int requestedQuantity)
    {
        // Get pricing for the specific combination
        var pricing = await _combinationPricingRepository
            .FindAsync(op => !op.IsDeleted && op.ItemCombinationId == itemCombinationId);

        if (pricing == null)
            throw new KeyNotFoundException("Combination pricing not found for itemCombinationId " + itemCombinationId);

        return new PricingResult
        {
            Price = pricing.Price,
            SalesPrice = pricing.SalesPrice,
            IsAvailable = pricing.AvailableQuantity > 0,
            ActiveOfferId = pricing.OfferId
        };
    }
}