namespace Shared.DTOs.Catalog.Item
{
    public class QuantityTierDto
    {
        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal SalesPricePerUnit { get; set; }
    }
}
