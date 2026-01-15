namespace Shared.DTOs.Catalog.Item
{
    public class CurrentCombinationDto
    {
        public Guid CombinationId { get; set; }
        public bool IsDefault { get; set; }
        public Guid CreatedBy { get; set; }
        public List<PricingAttributeDto>? PricingAttributes { get; set; }
        public List<ImageDto>? Images { get; set; }
    }
}
