using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.CMS
{
    public interface IOAuthService
    {
        /// <summary>
        /// Sign in or register user via Google OAuth.
        /// </summary>
        Task<ServiceResult<SignInResult>> GoogleSignInAsync(string idToken, string clientType);

        /// <summary>
        /// Sign in or register user via Facebook OAuth.
        /// </summary>
        Task<ServiceResult<SignInResult>> FacebookSignInAsync(string accessToken, string clientType);
    }
}
