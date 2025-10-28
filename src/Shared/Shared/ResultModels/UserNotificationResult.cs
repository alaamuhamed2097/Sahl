using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ResultModels
{
    public class UserNotificationResult<T>
    {
        public T Value { get; set; }
        public int TotalCount { get; set; }
        public int UnReadCount { get; set; }
    }
}
