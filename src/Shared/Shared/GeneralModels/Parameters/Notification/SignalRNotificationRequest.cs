using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GeneralModels.Parameters.Notification
{
    public class SignalRNotificationRequest
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "info,warning,error,success";
        public string GroupName { get; set; }
        public string UserId { get; set; }
    }
}
