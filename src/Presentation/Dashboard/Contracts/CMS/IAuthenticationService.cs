using Dashboard.Models;
using Shared.DTOs.User;
using Shared.DTOs.User.Customer;
using Shared.DTOs.User.OAuth;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;

namespace Dashboard.Contracts.CMS
{
    public interface IAuthenticationService
    {
        Task<ResponseModel<SignInResult>> Login(LoginRequestModel model);
        Task<ResponseModel<CustomerRegistrationResponseDto>> RegisterCustomer(CustomerRegistrationDto model);
        Task<ResponseModel<SignInResult>> GoogleOAuthSignIn(GoogleOAuthTokenDto tokenDto);
        Task<ResponseModel<SignInResult>> FacebookOAuthSignIn(FacebookOAuthTokenDto tokenDto);
        Task Logout();
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryRefreshTokenAsync();
        Task<ResponseModel<object>> SendResetPasswordCode(string email);
        Task<ResponseModel<object>> ResetPasswordWithCode(ResetPasswordWithCodeDto model);
    }
}
