using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GeneralModels.ResultModels
{
    public class ValidateRefreshTokenResult : OperationResult
    {
        public string UserId { get; set; }
        public List<string> UserRoles { get; set; } 
    }
}
