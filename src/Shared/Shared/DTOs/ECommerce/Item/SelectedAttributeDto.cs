namespace Shared.DTOs.ECommerce.Item
{
    public class SelectedAttributeDto
    {
        public Guid AttributeId { get; set; }
        public string AttributeNameAr { get; set; } = null!;
        public string AttributeNameEn { get; set; } = null!;
        public Guid CombinationValueId { get; set; }
        public string Value { get; set; }
    }
}
