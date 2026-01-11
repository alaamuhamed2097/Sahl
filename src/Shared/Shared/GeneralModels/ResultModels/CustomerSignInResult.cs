namespace Shared.GeneralModels.ResultModels
{
    public class CustomerSignInResult : SignInResult
    {
        public string PhoneCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
