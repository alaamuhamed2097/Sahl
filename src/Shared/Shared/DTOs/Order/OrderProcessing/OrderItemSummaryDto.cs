namespace Shared.DTOs.Order.OrderProcessing
{
    /// <summary>
    /// ملخص الـ Item في القائمة
    /// </summary>
    public class OrderItemSummaryDto
    {
        public string ItemName { get; set; } = null!;
        public string ThumbnailImage { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
