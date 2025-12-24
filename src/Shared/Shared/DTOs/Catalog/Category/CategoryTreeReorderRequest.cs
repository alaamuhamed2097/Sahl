namespace Shared.DTOs.Catalog.Category
{
    public class CategoryTreeReorderRequest
    {
        public Guid DraggedCategoryId { get; set; }
        public Guid TargetCategoryId { get; set; }
        public string DropPosition { get; set; } = "inside"; // "before", "inside", "after"
    }
}
