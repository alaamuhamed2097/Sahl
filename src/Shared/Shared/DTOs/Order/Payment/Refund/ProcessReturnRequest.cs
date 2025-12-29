namespace Shared.DTOs.Order.Payment.Refund
{
    public class ProcessReturnRequest
    {
        public bool Approved { get; set; }
        public string? AdminNotes { get; set; }
    }
}
