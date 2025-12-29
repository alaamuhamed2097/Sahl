using Shared.GeneralModels.ResultModels;

namespace BL.OperationResults
{
    public class BulkOperationResult : OperationResult
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public List<OperationResult> IndividualResults { get; set; } = new List<OperationResult>();
    }
}
