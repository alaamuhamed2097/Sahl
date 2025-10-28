using Resources;
using Resources.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.GeneralModels.Parameters.Notification
{
    public class UserNotificationRequest
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        [JsonIgnore]
        public string Title => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;
       
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [JsonIgnore]
        public string Description => ResourceManager.CurrentLanguage == Language.Arabic ? DescriptionAr : DescriptionEn;

        public bool IsRead { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public string TimeAgo { get; set; }
    }
}
