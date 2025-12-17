using Common.Enumerations.Fulfillment;
using Common.Enumerations.Offer;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Offer
{
    public class OfferDto : BaseDto
    {
        // Required properties
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }

        // Filtering and search optimization
        public int HandlingTimeInDays { get; set; }
        public OfferVisibilityScope VisibilityScope { get; set; }
        public FulfillmentType FulfillmentType { get; set; } = FulfillmentType.Seller;

        // Vendore performance metrics
        public decimal? VendorRatingForThisItem { get; set; }
        public int VendorSalesCountForThisItem { get; set; }
        public bool IsBuyBoxWinner { get; set; } = false;

        // Optional properties
        public Guid? WarrantyId { get; set; }

        // Collections
        public List<UserOfferRatingDto> UserOfferRatings { get; set; } = new List<UserOfferRatingDto>();
        public List<ShippingDetailDto> ShippingDetails { get; set; } = new List<ShippingDetailDto>();
        public List<OfferCombinationPricingDto> OfferCombinationPricings { get; set; } = new List<OfferCombinationPricingDto>();
        public List<OfferStatusHistoryDto> OfferStatusHistories { get; set; } = new List<OfferStatusHistoryDto>();
        public List<BuyBoxCalculationDto> BuyBoxCalculations { get; set; } = new List<BuyBoxCalculationDto>();
    }
}
