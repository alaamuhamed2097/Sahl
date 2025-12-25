using Shared.DTOs.Catalog.Category;
using Shared.DTOs.ECommerce;

namespace Dashboard.Services
{
    /// <summary>
    /// Lightweight service for Excel import (uses JavaScript for generation)
    /// </summary>
    public class ExcelTemplateService
    {
        // This service is kept for parsing only
        // Template generation is now handled by JavaScript (excelExportHelper.js)

        /// <summary>
        /// These methods are placeholders - actual generation happens in JavaScript
        /// </summary>
        public byte[] GenerateProductTemplate(
            List<CategoryDto> categories,
            List<CategoryAttributeDto> categoryAttributes,
            Dictionary<Guid, string> brands,
            Dictionary<Guid, string> units)
        {
            // JavaScript handles this now
            return Array.Empty<byte>();
        }

        public byte[] GenerateEmptyProductTemplate(
            List<CategoryDto> categories,
            List<CategoryAttributeDto> categoryAttributes)
        {
            // JavaScript handles this now
            return Array.Empty<byte>();
        }

        /// <summary>
        /// Parse Excel file - to be implemented later if needed
        /// For now, JavaScript can handle this too
        /// </summary>
        public List<ImportedProductDto> ParseProductsFromExcel(
            Stream fileStream,
            List<CategoryAttributeDto> categoryAttributes,
            List<CategoryDto> categories,
            Dictionary<Guid, string> brands,
            Dictionary<Guid, string> units)
        {
            // Placeholder - JavaScript will handle parsing
            return new List<ImportedProductDto>();
        }
    }

    /// <summary>
    /// DTO for imported product data from Excel
    /// </summary>
    public class ImportedProductDto
    {
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string ShortDescriptionAr { get; set; } = string.Empty;
        public string ShortDescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool StockStatus { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }
        public string? ThumbnailImagePath { get; set; }
        public List<string> ImagePaths { get; set; } = new();
        public Dictionary<Guid, string> AttributeValues { get; set; } = new();
        public Dictionary<Guid, List<Guid>> PricingAttributeValues { get; set; } = new();
        public int RowNumber { get; set; }
    }
}
