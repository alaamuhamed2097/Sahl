using Resources;
using Resources.Enumerations;

namespace Shared.DTOs.ECommerce.Category
{
    public class VwAttributeWithOptionsDto
    {
        public Guid AttributeId { get; set; }
        public string AttributeTitleAr { get; set; } = null!;
        public string AttributeTitleEn { get; set; } = null!;
        public string Title() =>
           ResourceManager.CurrentLanguage == Language.Arabic ? AttributeTitleAr : AttributeTitleEn;
        public int FieldType { get; set; }
        public int MaxLength { get; set; }
        public List<AttributeOptionJsonModel> Options { get; set; }
    }
}
