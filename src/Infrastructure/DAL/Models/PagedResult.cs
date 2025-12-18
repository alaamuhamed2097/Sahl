namespace DAL.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int TotalRecords { get; }

        public PagedResult(IEnumerable<T> items, int totalRecords)
        {
            Items = items.ToList();
            TotalRecords = totalRecords;
        }
    }
}
