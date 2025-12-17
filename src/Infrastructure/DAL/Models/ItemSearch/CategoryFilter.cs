namespace DAL.Models.ItemSearch;

public class CategoryFilter
{
    public Guid Id { get; set; }
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public string Icon { get; set; }
    public int ItemCount { get; set; }
}
