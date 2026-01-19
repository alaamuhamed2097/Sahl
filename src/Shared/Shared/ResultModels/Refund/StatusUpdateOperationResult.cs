namespace Shared.ResultModels.Refund
{
    public class StatusUpdateOperationResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public static StatusUpdateOperationResult Success() =>
            new StatusUpdateOperationResult { IsSuccess = true };

        public static StatusUpdateOperationResult Fail(string errorMessage) =>
            new StatusUpdateOperationResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
