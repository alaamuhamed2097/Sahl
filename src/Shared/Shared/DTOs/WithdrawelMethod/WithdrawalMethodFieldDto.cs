using Common.Enumerations.FieldType;
using Shared.Attributes;
using Shared.DTOs.Base;

namespace Shared.DTOs.WithdrawalMethod
{
    public class WithdrawalMethodFieldDto : BaseDto
    {
        public Guid FieldId { get; set; }
        public Guid UserWithdrawalMethodId { get; set; } = Guid.Empty;
        public FieldType FieldType { get; set; }

        [FieldValueValidation]
        public string Value { get; set; } = string.Empty;
    }
}
