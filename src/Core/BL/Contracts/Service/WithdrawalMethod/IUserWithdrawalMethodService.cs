using BL.Contracts.Service.Base;
using Domains.Entities.WithdrawalMethods;
using Shared.DTOs.WithdrawalMethod;

namespace BL.Contracts.Service.WithdrawalMethod
{
    public interface IUserWithdrawalMethodService : IBaseService<TbUserWithdrawalMethod, UserWithdrawalMethodDto>
    {
        public Task<IEnumerable<WithdrawalMethodsFieldsValuesDto>> GetAllWithdrawalFieldsValues(string UserId);
        public Task<WithdrawalMethodsFieldsValuesDto> FindWithdrawalFieldsValuesById(Guid WithdrawalMethodId, string UserId);

    }
}
