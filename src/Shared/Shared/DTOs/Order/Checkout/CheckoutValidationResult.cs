namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Checkout validation result
    /// </summary>
    public class CheckoutValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}