using Dashboard.Models;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;

namespace Dashboard.Contracts.General
{
    public interface IAuthenticationService
    {
        Task<ResponseModel<SignInResult>> Login(LoginRequestModel model);
        Task Logout();
        Task<bool> TryRefreshTokenAsync();
    }
}
