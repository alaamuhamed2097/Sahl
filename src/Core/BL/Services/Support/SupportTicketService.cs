using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Support;
using Shared.DTOs.Support;

namespace BL.Services.Support;

public interface ISupportTicketService : IBaseService<TbSupportTicket, SupportTicketDto>
{
}

public class SupportTicketService : BaseService<TbSupportTicket, SupportTicketDto>, ISupportTicketService
{
    public SupportTicketService(ITableRepository<TbSupportTicket> repository, IBaseMapper mapper)
        : base(repository, mapper)
    {
    }
}
