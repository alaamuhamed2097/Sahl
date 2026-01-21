namespace Shared.ResultModels.Refund
{
    public class RefundExecutionResult
    {
        public bool IsSuccess { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? ErrorMessage { get; set; }

        public static RefundExecutionResult Success(string refundTransactionId) =>
            new RefundExecutionResult { IsSuccess = true, RefundTransactionId = refundTransactionId };

        public static RefundExecutionResult Fail(string errorMessage) =>
            new RefundExecutionResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
