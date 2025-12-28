using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Support;
using Shared.DTOs.Support;

namespace BL.Services.Support;

public interface IDisputeService : IBaseService<TbDispute, DisputeDto>
{
}

public class DisputeService : BaseService<TbDispute, DisputeDto>, IDisputeService
{
    public DisputeService(ITableRepository<TbDispute> repository, IBaseMapper mapper)
        : base(repository, mapper)
    {
    }
}
