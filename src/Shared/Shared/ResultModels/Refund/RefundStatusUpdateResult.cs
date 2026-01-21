using Common.Enumerations.Order;

namespace Shared.ResultModels.Refund
{
    public class RefundStatusUpdateResult
    {
        public bool IsSuccess { get; set; }
        public Guid? RefundId { get; set; }
        public RefundStatus? OldStatus { get; set; }
        public RefundStatus? NewStatus { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? ErrorMessage { get; set; }

        public static RefundStatusUpdateResult Success(
            Guid refundId,
            RefundStatus oldStatus,
            RefundStatus newStatus,
            string? transactionId = null) =>
            new RefundStatusUpdateResult
            {
                IsSuccess = true,
                RefundId = refundId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                RefundTransactionId = transactionId
            };

        public static RefundStatusUpdateResult Fail(string errorMessage) =>
            new RefundStatusUpdateResult { IsSuccess = false, ErrorMessage = errorMessage };
    }

}
