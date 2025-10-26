using Common.Enumerations;
using Domains.Entities.Order;
using Microsoft.AspNetCore.Identity;

namespace Domains.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfileImagePath { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public UserStateType UserState { get; set; }

        public virtual HashSet<TbOrder> Orders { get; set; } = new HashSet<TbOrder>();
    }
}
