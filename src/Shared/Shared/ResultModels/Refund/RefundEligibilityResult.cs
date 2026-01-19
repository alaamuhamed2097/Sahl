namespace Shared.ResultModels.Refund
{
    public class RefundEligibilityResult
    {
        public bool IsEligible { get; set; }
        public string? Reason { get; set; }

        public static RefundEligibilityResult Eligible() =>
            new RefundEligibilityResult { IsEligible = true };

        public static RefundEligibilityResult NotEligible(string reason) =>
            new RefundEligibilityResult { IsEligible = false, Reason = reason };
    }
}
