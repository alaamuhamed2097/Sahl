using Common.Enumerations.User;
using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;
using System.Security.Claims;

namespace BL.Contracts.GeneralService.CMS;

public interface IUserTokenService
{
    Task<GenerateTokenResult> GenerateJwtTokenAsync(string userId, IList<string> roles);
    ClaimsPrincipal? ValidateJwtToken(string token);
    Task<string> CreateRefreshTokenAsync(ApplicationUser user, string clientType);
    Task<ValidateRefreshTokenResult> ValidateRefreshTokenAsync(RefreshTokenDto refreshToken, string clientType);
    Task<RegenerateRefreshTokenResult> RegenerateRefreshTokenAsync(RefreshTokenDto refreshTokenDto, string clientType);
    Task<UserStateType> GetUserStateAsync(string userId);
}
