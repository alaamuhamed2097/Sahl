using Shared.DTOs.User.Admin;
using Shared.DTOs.User.Customer;
using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.UserManagement;

public interface IUserRegistrationService
{
    Task<OperationResult> RegisterAdminAsync(AdminRegistrationDto userDto, Guid CreatorId);
    Task<ServiceResult<CustomerRegistrationResponseDto>> RegisterCustomerAsync(CustomerRegistrationDto userDto, string clientType);
}
