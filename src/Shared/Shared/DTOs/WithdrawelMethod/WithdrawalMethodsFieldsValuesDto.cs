using Resources;
using Resources.Enumerations;

namespace Shared.DTOs.WithdrawalMethod
{
    public class WithdrawalMethodsFieldsValuesDto
    {
        public Guid WithdrawalMethodId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Title()
           => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;
        public string ImagePath { get; set; }
        public List<FieldValueModel>? FieldsJson { get; set; }
    }
}
