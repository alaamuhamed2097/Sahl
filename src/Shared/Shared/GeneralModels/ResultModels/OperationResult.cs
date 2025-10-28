using Common.Enumerations;

namespace Shared.GeneralModels.ResultModels
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string? ErrorCode { get; set; }
        public AlertType Alert { get; set; }
    }
}
