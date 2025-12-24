using BL.Contracts.Service.Catalog.Pricing;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Shared.DTOs.Catalog.Pricing;

namespace BL.Service.Catalog.Pricing
{
    public class PricingService : IPricingService
    {
        private readonly IEnumerable<IPricingStrategy> _strategies;

        public PricingService(IEnumerable<IPricingStrategy> strategies)
        {
            _strategies = strategies;
        }

        public PricingResult CalculatePrice(PricingContext context)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(context.Strategy));

            if (strategy == null)
            {
                throw new InvalidOperationException($"No pricing strategy found for type {context.Strategy}");
            }

            return strategy.CalculatePrice(context);
        }

        public PricingResult GetBestOffer(TbItemCombination combination, int quantity = 1)
        {
            // Determine strategy from category or item configuration
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
