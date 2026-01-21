namespace Shared.ResultModels.Refund
{
    public class RefundRequestResult
    {
        public bool IsSuccess { get; set; }
        public string? RefundNumber { get; set; }
        public string? ErrorMessage { get; set; }

        public static RefundRequestResult Success(string refundNumber) =>
            new RefundRequestResult { IsSuccess = true, RefundNumber = refundNumber };

        public static RefundRequestResult Fail(string errorMessage) =>
            new RefundRequestResult { IsSuccess = false, ErrorMessage = errorMessage };
    }

}
