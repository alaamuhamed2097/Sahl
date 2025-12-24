using BL.Contracts.Service.Catalog.Pricing;
using Common.Enumerations.Pricing;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Service.Catalog.Pricing
{
    public class PricingService : IPricingService
    {
        private readonly Dictionary<PricingStrategyType, IPricingStrategy> _strategies;

        public PricingService(
            SimplePricingStrategy simplePricing,
            CombinationBasedPricingStrategy combinationPricing,
            QuantityBasedPricingStrategy quantityPricing,
            HybridPricingStrategy hybridPricing)
        {
            _strategies = new Dictionary<PricingStrategyType, IPricingStrategy>
            {
                { PricingStrategyType.Simple, simplePricing },
                { PricingStrategyType.CombinationBased, combinationPricing },
                { PricingStrategyType.QuantityBased, quantityPricing },
                { PricingStrategyType.Hybrid, hybridPricing }
            };
        }

        public PricingResult CalculatePrice(PricingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!_strategies.TryGetValue(context.Strategy, out var strategy))
            {
                throw new InvalidOperationException(
                    $"No pricing strategy found for type {context.Strategy}");
            }

            return strategy.CalculatePrice(context);
        }

        public PricingResult GetBestOffer(TbItemCombination combination, int quantity = 1)
        {
            if (combination == null)
            {
                throw new ArgumentNullException(nameof(combination));
            }

            if (quantity < 1)
            {
                throw new ArgumentException("Quantity must be at least 1", nameof(quantity));
            }

            var strategyType = combination.Item.Category.PricingSystemType;

            var context = new PricingContext
            {
                ItemCombination = combination,
                RequestedQuantity = quantity,
                Strategy = strategyType,
                CalculationDate = DateTime.UtcNow
            };

            return CalculatePrice(context);
        }
    }
}