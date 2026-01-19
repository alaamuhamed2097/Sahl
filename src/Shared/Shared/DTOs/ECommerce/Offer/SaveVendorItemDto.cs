using Common.Enumerations.Fulfillment;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Offer
{
    public class SaveVendorItemDto : BaseDto
    {
        // Required properties
        public Guid ItemId { get; set; }
        public Guid WarehouseId { get; set; }

        // Filtering and search optimization
        public int? EstimatedDeliveryDays { get; set; }
        public bool IsFreeShipping { get; set; } = false;
        public FulfillmentType FulfillmentType { get; set; } = FulfillmentType.Marketplace;

        // Optional properties
        public Guid? WarrantyId { get; set; }

        // Collections
        public List<OfferCombinationPricingDto>? OfferCombinationPricings { get; set; } 
    }
}
