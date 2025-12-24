using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;


namespace Shared.DTOs.Catalog.Unit
{
    public class ConversionUnitDto : BaseDto
    {
        public Guid ConversionUnitId { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; } = null!;
        public double ConversionFactor { get; set; }
        public string Title()
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;
    }
}
