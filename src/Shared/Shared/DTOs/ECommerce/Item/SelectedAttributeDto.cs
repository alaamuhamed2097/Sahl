namespace Shared.DTOs.ECommerce.Item
{
    public class PricingAttributeDto
    {
        public Guid AttributeId { get; set; }
        public string AttributeNameAr { get; set; } = null!;
        public string AttributeNameEn { get; set; } = null!;
        public Guid CombinationValueId { get; set; }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
        public bool IsSelected { get; set; }
    }
}
