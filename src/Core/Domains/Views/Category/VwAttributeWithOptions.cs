namespace Domains.Views.Category
{
    public class VwAttributeWithOptions
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int FieldType { get; set; }
        public bool IsRangeFieldType { get; set; } = false;
        public int? MaxLength { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string? AttributeOptionsJson { get; set; }

    }
}
