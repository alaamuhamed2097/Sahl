namespace Common.Filters
{
    public class AvailableFiltersQuery
    {
        public string SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? VendorId { get; set; }
    }
}
