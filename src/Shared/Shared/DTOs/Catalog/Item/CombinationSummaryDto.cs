namespace Shared.DTOs.Catalog.Item
{
    public class CombinationSummaryDto
    {
        public int TotalVendors { get; set; }
        public bool IsMultiVendor { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal AvgPrice { get; set; }
        public int TotalStock { get; set; }
    }
}
