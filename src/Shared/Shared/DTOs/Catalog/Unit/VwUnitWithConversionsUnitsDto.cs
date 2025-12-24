namespace Shared.DTOs.Catalog.Unit
{
    public class VwUnitWithConversionsUnitsDto
    {
        public Guid UnitId { get; set; }
        public string UnitTitleAr { get; set; }
        public string UnitTitleEn { get; set; }
        public List<ConversionUnitDto>? ConversionUnitsFromJson { get; set; }
        public List<ConversionUnitDto>? ConversionUnitsToJson { get; set; }
    }
}
