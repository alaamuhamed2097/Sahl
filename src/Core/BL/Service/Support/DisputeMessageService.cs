using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Support;
using Shared.DTOs.Support;

namespace BL.Service.Support
{
    public interface IDisputeMessageService : IBaseService<TbDisputeMessage, DisputeMessageDto>
    {
    }

    public class DisputeMessageService : BaseService<TbDisputeMessage, DisputeMessageDto>, IDisputeMessageService
    {
        public DisputeMessageService(ITableRepository<TbDisputeMessage> repository, IBaseMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
