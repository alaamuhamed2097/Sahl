using Common.Enumerations.User;
using Domains.Entities.Loyalty;
using Domains.Entities.Order.Cart;
using Domains.Entities.Order.Refund;
using Domains.Entities.Wallet.Customer;
using Domains.Entities.WithdrawalMethods;
using Microsoft.AspNetCore.Identity;

namespace Domains.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => $"{FirstName} {LastName}";
        public string ProfileImagePath { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;

        /// <summary>
        /// Normalized phone in format: {PhoneCode}{PhoneNumber}
        /// Used for unique constraint and phone-based lookups
        /// </summary>
        public string? NormalizedPhone { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public UserStateType UserState { get; set; }

        public virtual ICollection<TbUserWithdrawalMethod> UserWithdrawalMethods { get; set; }
        public virtual ICollection<TbCustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<TbShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<TbCustomerLoyalty> CustomerLoyalties { get; set; }
        public virtual ICollection<TbCustomerWallet> CustomerWallets { get; set; }
        public virtual ICollection<TbOrder> Orders { get; set; } = new List<TbOrder>();
        public virtual ICollection<TbRefund> Refunds { get; set; } = new List<TbRefund>();
    }
}
