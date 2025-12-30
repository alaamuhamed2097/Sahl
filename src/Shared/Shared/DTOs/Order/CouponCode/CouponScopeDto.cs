using Common.Enumerations.Order;

namespace Shared.DTOs.Order.CouponCode
{
    /// <summary>
    /// Scope DTO for Categories or Products
    /// </summary>
    public class CouponScopeDto
    {
        public Guid Id { get; set; }
        public CouponCodeScopeType ScopeType { get; set; }
        public Guid ScopeId { get; set; }
    }
}
