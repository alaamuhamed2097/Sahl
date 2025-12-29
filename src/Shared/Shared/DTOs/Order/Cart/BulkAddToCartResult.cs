namespace Shared.DTOs.Order.Cart
{
    public class BulkAddToCartResult
    {
        public List<BulkAddItemResult> SuccessfulItems { get; set; }
        public List<BulkAddItemFailure> FailedItems { get; set; }
        public CartSummaryDto CartSummary { get; set; }
        public int TotalItemsAdded { get; set; }
        public int TotalItemsFailed { get; set; }
        public bool IsFullSuccess { get; }
        public bool IsPartialSuccess { get; }
        public bool IsCompleteFailure { get; }
    }
}