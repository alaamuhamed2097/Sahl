namespace Shared.GeneralModels.ResultModels
{
    public class VendorSignInResult : SignInResult
    {
        public string Email { get; set; } = null!;
        public bool IsVerified { get; set; }
    }
}
