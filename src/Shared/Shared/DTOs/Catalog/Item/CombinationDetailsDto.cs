namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// Response for POST /api/items/{id}/combination
    /// </summary>
    public class CombinationDetailsDto
    {
        public Guid? CombinationId { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public bool IsAvailable { get; set; }
        public string Message { get; set; }

        public List<PricingAttributeDto> PricingAttributes { get; set; }
        public List<ItemImageDto> Images { get; set; }
        public List<VendorOfferDto> Offers { get; set; }
        public CombinationSummaryDto Summary { get; set; }

        /// <summary>
        /// If incomplete selection, shows missing attributes
        /// </summary>
        public List<MissingAttributeDto> MissingAttributes { get; set; }
    }
}
