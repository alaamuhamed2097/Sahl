namespace DAL.Models.ItemSearch;

public class VendorFilter
{
    public Guid Id { get; set; }
    public string StoreName { get; set; }
    public int ItemCount { get; set; }
    public decimal? AvgRating { get; set; }
}
