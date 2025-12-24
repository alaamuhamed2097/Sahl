namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Category card for showcase blocks
    /// </summary>
    public class CategoryCardDto
    {
        public Guid CategoryId { get; set; }

        // Bilingual names
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}