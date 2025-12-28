using DAL.Models;
using Domains.Entities.Order;

namespace DAL.Contracts.Repositories.Order
{
    public interface IOrderRepository : ITableRepository<TbOrder>
    {
        /// <summary>
        /// Get order with all related entities (OrderDetails, Items, Vendors, Address)
        /// </summary>
        Task<TbOrder?> GetOrderWithDetailsAsync(Guid orderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get order with shipments
        /// </summary>
        Task<TbOrder?> GetOrderWithShipmentsAsync(Guid orderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get customer orders with pagination
        /// </summary>
        Task<PagedResult<TbOrder>> GetCustomerOrdersPagedAsync(
            string customerId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}