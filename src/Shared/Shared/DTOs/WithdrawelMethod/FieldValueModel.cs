using Common.Enumerations.FieldType;
using Resources;
using Resources.Enumerations;
using Shared.Attributes;


namespace Shared.DTOs.WithdrawalMethod
{
    public class FieldValueModel
    {
        public Guid Id { get; set; }
        public Guid UserWithdrawalMethodId { get; set; }
        public Guid FieldId { get; set; }
        public string? UserId { get; set; }
        public string FieldTitleAr { get; set; } = null!;
        public string FieldTitleEn { get; set; } = null!;
        public string Title()
        => ResourceManager.CurrentLanguage == Language.Arabic ? FieldTitleAr : FieldTitleEn;
        public FieldType FieldType { get; set; }

        [FieldValueValidation]
        public string? Value { get; set; }
    }
}
