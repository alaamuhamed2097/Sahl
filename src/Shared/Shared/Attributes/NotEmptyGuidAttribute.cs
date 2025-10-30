using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attributes
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public NotEmptyGuidAttribute()
        {
            ErrorMessageResourceName = "FieldRequired";
            ErrorMessageResourceType = typeof(ValidationResources);
        }

        public override bool IsValid(object value)
        {
            if (value is Guid guid)
            {
                return guid != Guid.Empty;
            }
            return true; // Let [Required] handle null checks
        }
    }
}
