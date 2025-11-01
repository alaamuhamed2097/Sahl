namespace Shared.GeneralModels.Parameters
{
    public class TokenRefreshResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
}
