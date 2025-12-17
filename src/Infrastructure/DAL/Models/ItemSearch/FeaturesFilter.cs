namespace DAL.Models.ItemSearch;

public class FeaturesFilter
{
    public int FreeShippingCount { get; set; }
    public int HasFreeShipping { get; set; }
    public int WithWarrantyCount { get; set; }
    public int HasWarranty { get; set; }
    public int InStockCount { get; set; }
    public int HasInStock { get; set; }
    public int TotalItems { get; set; }
}