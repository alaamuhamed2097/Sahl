using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using DAL.Models;
using Domains.Entities.Order.Shipping;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Order
{
    /// <summary>
    /// FIXED: Corrected based on actual entity structure
    /// </summary>
    public class ShipmentRepository : TableRepository<TbOrderShipment>, IShipmentRepository
    {
        public ShipmentRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
        {
        }

        public async Task<TbOrderShipment?> GetShipmentWithDetailsAsync(
            Guid shipmentId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrderShipment>()
                    .AsNoTracking()
                    .Where(s => s.Id == shipmentId && !s.IsDeleted)
                    .Include(s => s.Order)
                        .ThenInclude(o => o.User)
                    .Include(s => s.Order)
                        .ThenInclude(o => o.CustomerAddress)
                            .ThenInclude(a => a.City)
                                .ThenInclude(c => c.State) // ✅ State instead of Governorate
                    .Include(s => s.Vendor)
                        .ThenInclude(v => v.User)
                    .Include(s => s.Warehouse)
                    .Include(s => s.ShippingCompany)
                    .Include(s => s.Items)
                        .ThenInclude(si => si.Item)
                            .ThenInclude(i => i.ItemImages) // ✅ ItemImages not Images
                    .Include(s => s.Items)
                        .ThenInclude(si => si.OrderDetail)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting shipment with details for shipment {ShipmentId}", shipmentId);
                throw;
            }
        }

        public async Task<List<TbOrderShipment>> GetOrderShipmentsAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrderShipment>()
                    .AsNoTracking()
                    .Where(s => s.OrderId == orderId && !s.IsDeleted)
                    .Include(s => s.Vendor)
                        .ThenInclude(v => v.User)
                    .Include(s => s.Warehouse)
                    .Include(s => s.ShippingCompany)
                    .Include(s => s.Items)
                        .ThenInclude(si => si.Item)
                            .ThenInclude(i => i.ItemImages) // ✅ ItemImages not Images
                    .OrderBy(s => s.CreatedDateUtc)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting shipments for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<PagedResult<TbOrderShipment>> GetVendorShipmentsPagedAsync(
            Guid vendorId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Set<TbOrderShipment>()
                    .AsNoTracking()
                    .Where(s => s.VendorId == vendorId && !s.IsDeleted)
                    .Include(s => s.Order)
                        .ThenInclude(o => o.User)
                    .Include(s => s.Items)
                        .ThenInclude(si => si.Item)
                            .ThenInclude(i => i.ItemImages) // ✅ ItemImages not Images
                    .OrderByDescending(s => s.CreatedDateUtc);

                var totalCount = await query.CountAsync(cancellationToken);

                var shipments = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return new PagedResult<TbOrderShipment>(shipments, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting paged shipments for vendor {VendorId}", vendorId);
                throw;
            }
        }

        public async Task<TbOrderShipment?> GetShipmentByTrackingNumberAsync(
            string trackingNumber,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrderShipment>()
                    .AsNoTracking()
                    .Where(s => s.TrackingNumber == trackingNumber && !s.IsDeleted)
                    .Include(s => s.Order)
                        .ThenInclude(o => o.CustomerAddress)
                            .ThenInclude(a => a.City)
                                .ThenInclude(c => c.State) // ✅ State instead of Governorate
                    .Include(s => s.ShippingCompany)
                    .Include(s => s.Vendor)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting shipment by tracking number {TrackingNumber}", trackingNumber);
                throw;
            }
        }

        public async Task<List<TbShipmentStatusHistory>> GetShipmentStatusHistoryAsync(
            Guid shipmentId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbShipmentStatusHistory>()
                    .AsNoTracking()
                    .Where(h => h.ShipmentId == shipmentId && !h.IsDeleted)
                    .OrderBy(h => h.StatusDate)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting status history for shipment {ShipmentId}", shipmentId);
                throw;
            }
        }
    }
}