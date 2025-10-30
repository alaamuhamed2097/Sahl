using Resources;
using Resources.Enumerations;


namespace Shared.DTOs.ECommerce.Category
{
    public class AttributeOptionJsonModel
    {
        public Guid Id { get; set; }
        public Guid AttributeId { get; set; }
        public string OptionTitleAr { get; set; } = null!;
        public string OptionTitleEn { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public string Title()
        => ResourceManager.CurrentLanguage == Language.Arabic ? OptionTitleAr : OptionTitleEn;
    }
}
