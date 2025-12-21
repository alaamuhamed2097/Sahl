using Common.Enumerations.Fulfillment;
using Common.Enumerations.Offer;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferDto : BaseDto
    {
        // Required properties
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }

        // Filtering and search optimization
        public int HandlingTimeInDays { get; set; }
        public bool IsFreeShipping { get; set; } = false;
        public OfferVisibilityScope VisibilityScope { get; set; }
        public FulfillmentType FulfillmentType { get; set; } = FulfillmentType.Seller;

        // Vendor performance metrics
        public bool IsBuyBoxWinner { get; set; } = false;

        // Optional properties
        public Guid? WarrantyId { get; set; }

        // Collections
        public List<UserOfferRatingDto> UserOfferRatings { get; set; } = new List<UserOfferRatingDto>();
        public List<OfferCombinationPricingDto> OfferCombinationPricings { get; set; } = new List<OfferCombinationPricingDto>();
    }
}
