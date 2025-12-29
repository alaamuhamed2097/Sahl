using Domains.Entities.Order.Cart;

namespace DAL.ResultModels
{
    /// <summary>
    /// Result model for cart transaction operations
    /// </summary>
    public class CartTransactionResult : SaveResult
    {
        public TbShoppingCart Cart { get; set; }
        public int TotalItems { get; set; }
        public decimal CartTotal { get; set; }
        public List<Guid> AffectedItemIds { get; set; } = new();
    }
}
