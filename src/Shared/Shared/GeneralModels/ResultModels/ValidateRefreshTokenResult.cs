namespace Shared.GeneralModels.ResultModels
{
    public class ValidateRefreshTokenResult : OperationResult
    {
        public string UserId { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
