using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Order.Shipping;

namespace DAL.Contracts.Repositories.Order
{
    public interface IShipmentRepository : ITableRepository<TbOrderShipment>
    {
        /// <summary>
        /// Get shipment with all related data (Items, Order, Vendor, Warehouse)
        /// </summary>
        Task<TbOrderShipment?> GetShipmentWithDetailsAsync(Guid shipmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all shipments for an order
        /// </summary>
        Task<List<TbOrderShipment>> GetOrderShipmentsAsync(Guid orderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get vendor shipments with pagination
        /// </summary>
        Task<PagedResult<TbOrderShipment>> GetVendorShipmentsPagedAsync(
            Guid vendorId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get shipment by tracking number with full details
        /// </summary>
        Task<TbOrderShipment?> GetShipmentByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get shipment status history
        /// </summary>
        Task<List<TbShipmentStatusHistory>> GetShipmentStatusHistoryAsync(Guid shipmentId, CancellationToken cancellationToken = default);
    }
}