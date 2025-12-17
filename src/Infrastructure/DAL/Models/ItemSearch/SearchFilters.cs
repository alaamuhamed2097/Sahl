namespace DAL.Models.ItemSearch;

public class SearchFilters
{
    public List<CategoryFilter> Categories { get; set; }
    public List<BrandFilter> Brands { get; set; }
    public List<VendorFilter> Vendors { get; set; }
    public PriceRangeFilter PriceRange { get; set; }
    public List<AttributeFilter> Attributes { get; set; }
    public List<ConditionFilter> Conditions { get; set; }
    public FeaturesFilter Features { get; set; }
}
