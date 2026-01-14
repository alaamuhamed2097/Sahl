using Common.Enumerations.FieldType;

namespace Shared.DTOs.Catalog.Item
{
    //public class ItemImageDto
    //{
    //    public string ImageUrl { get; set; }
    //    public int DisplayOrder { get; set; }
    //    public bool IsDefault { get; set; }
    //}

    // <summary>
    // Can be either pricing attribute(with options) or spec attribute(with value)
    // </summary>
    public class ItemAttributeDefinitionDto
    {
        public Guid AttributeId { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public FieldType FieldType { get; set; }
        public int DisplayOrder { get; set; }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
    }
}
