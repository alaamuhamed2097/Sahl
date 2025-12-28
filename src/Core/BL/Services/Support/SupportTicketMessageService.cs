using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Support;
using Shared.DTOs.Support;

namespace BL.Services.Support;

public interface ISupportTicketMessageService : IBaseService<TbSupportTicketMessage, SupportTicketMessageDto>
{
}

public class SupportTicketMessageService : BaseService<TbSupportTicketMessage, SupportTicketMessageDto>, ISupportTicketMessageService
{
    public SupportTicketMessageService(ITableRepository<TbSupportTicketMessage> repository, IBaseMapper mapper)
        : base(repository, mapper)
    {
    }
}
