namespace Domains.Procedures
{
    /// <summary>
    /// Result from SpGetCombinationByAttributes
    /// Single row with JSON columns
    /// </summary>
    public class SpGetAvailableOptionsForSelection
    {
        public Guid? CombinationId { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public bool IsAvailable { get; set; }
        public string Message { get; set; }

        // JSON Columns
        public string SelectedAttributesJson { get; set; }
        public string ImagesJson { get; set; }
        public string OffersJson { get; set; }
        public string SummaryJson { get; set; }
        public string MissingAttributesJson { get; set; }
    }
}
