using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.User.Admin
{
    public class AdminProfileUpdateDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\p{L}+(\s\p{L}+)*$", ErrorMessageResourceName = "NameFormat", ErrorMessageResourceType = typeof(ValidationResources))]

        public string FirstName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(20, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(@"^\p{L}+(\s\p{L}+)*$", ErrorMessageResourceName = "NameFormat", ErrorMessageResourceType = typeof(ValidationResources))]

        public string LastName { get; set; } = null!;
    }
}