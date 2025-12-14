using Domains.Entities.Order;

namespace DAL.Contracts.Repositories;

public interface IOrderRepository : ITableRepository<TbOrder>
{
    Task<TbOrder?> FindByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<List<TbOrder>> GetCustomerOrdersAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}

