namespace DAL.Models.ItemSearch;

public class AttributeFilter
{
    public Guid AttributeId { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int DisplayOrder { get; set; }
    public List<AttributeValueFilter> Values { get; set; }
}
