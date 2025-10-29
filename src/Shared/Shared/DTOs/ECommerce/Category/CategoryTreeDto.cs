namespace Shared.DTOs.ECommerce.Category
{
    public class CategoryTreeDto : CategoryDto
    {
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}
