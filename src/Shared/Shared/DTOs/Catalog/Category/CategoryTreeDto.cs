namespace Shared.DTOs.Catalog.Category
{
    public class CategoryTreeDto : CategoryDto
    {
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}
