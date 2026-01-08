using BL.Contracts.Service.Base;
using Domains.Entities.WithdrawalMethods;
using Shared.DTOs.WithdrawalMethod;

namespace BL.Contracts.Service.WithdrawalMethod
{

    public interface IWithdrawalMethodService : IBaseService<TbWithdrawalMethod, WithdrawalMethodDto>
    {
    }
}
