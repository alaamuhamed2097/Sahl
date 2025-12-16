namespace Shared.DTOs.ECommerce.Item
{
    public class FeaturesFilterDto
    {
        public int FreeShippingCount { get; set; }
        public bool HasFreeShipping { get; set; }
        public int WithWarrantyCount { get; set; }
        public bool HasWarranty { get; set; }
        public int InStockCount { get; set; }
        public bool HasInStock { get; set; }
        public int TotalItems { get; set; }
    }
}
