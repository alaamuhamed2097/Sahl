namespace Shared.DTOs.ECommerce.Item
{
    public class AttributeFilterDto
    {
        public Guid AttributeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int DisplayOrder { get; set; }
        public List<AttributeValueFilterDto> Values { get; set; }
    }
}
