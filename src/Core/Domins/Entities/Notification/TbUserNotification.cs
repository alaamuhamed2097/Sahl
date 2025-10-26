using Domains.Entities.Base;
using Domains.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Notification
{
    public class TbUserNotification : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(TbNotification))]
        public Guid NotificationId { get; set; }

        [Required]
        public bool IsRead { get; set; } = false; 

        public virtual ApplicationUser User { get; set; }
        public virtual TbNotification TbNotification { get; set; }
    }
}