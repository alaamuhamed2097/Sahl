namespace DAL.Models.ItemSearch;

public class VendorFilter
{
    public Guid Id { get; set; }
    public string StoreName { get; set; }
    public string StoreNameAr { get; set; }
    public string LogoUrl { get; set; }
    public int ItemCount { get; set; }
    public decimal? AvgRating { get; set; }
}
