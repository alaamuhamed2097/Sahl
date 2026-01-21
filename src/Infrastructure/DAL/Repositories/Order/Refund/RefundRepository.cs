using BL.Contracts.GeneralService;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Filters;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using DAL.Exceptions;
using DAL.ResultModels;
using Domains.Entities.Order;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using Domains.Views.Order.Refund;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Repositories.Order.Refund;

/// <summary>
/// Repository implementation for Refund operations
/// Implements refund-specific query methods
/// </summary>
public class RefundRepository : IRefundRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    protected readonly ILogger _logger;
    public RefundRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _logger = logger;
    }
    /// <summary>
    /// Get refund by ID from database.
    /// </summary>
    public async Task<TbRefund> GetByIdAsync(Guid id)
    {
        try
        {
            var data = await _dbContext.Set<TbRefund>()
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (data == null)
                throw new NotFoundException($"Entity of type {typeof(TbRefund).Name} with ID {id} not found.", _logger);
            return data;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting refund by ID: {RefundId}", id);
            throw;
        }
    }

    /// <summary>
    /// Get refund details view by ID.
    /// </summary>
    public async Task<VwRefundDetails> GetDetailsByIdAsync(Guid id)
    {
        try
        {
            var data = await _dbContext.Set<VwRefundDetails>()
                .FirstOrDefaultAsync(r => r.Id == id);
            if (data == null)
                throw new NotFoundException($"Entity of type {typeof(VwRefundDetails).Name} with ID {id} not found.", _logger);
            return data;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting refund details by ID: {RefundId}", id);
            throw;
        }
    }

    /// <summary>
    /// Get refund by number from database.
    /// </summary>
    public async Task<TbRefund> GetByNumberAsync(string number)
    {
        try
        {
            var data = await _dbContext.Set<TbRefund>()
                .FirstOrDefaultAsync(r => r.Number == number && !r.IsDeleted);
            if (data == null)
                throw new NotFoundException($"Entity of type {typeof(TbRefund).Name} with Number {number} not found.", _logger);
            return data;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting refund by number: {Number}", number);
            throw;
        }
    }

    /// <summary>
    /// Get refund by order detail ID from database.
    /// </summary>
    public async Task<TbRefund> GetByOrderDetailIdAsync(Guid orderDetailId)
    {
        try
        {
            var data = await _dbContext.Set<TbRefund>()
                .FirstOrDefaultAsync(r => r.OrderDetailId == orderDetailId && !r.IsDeleted);
            if (data == null)
                throw new NotFoundException($"Entity of type {typeof(TbRefund).Name} with OrderDetailId {orderDetailId} not found.", _logger);
            return data;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting refund by order detail ID: {OrderDetailId}", orderDetailId);
            throw;
        }
    }
    /// <summary>
    /// Asynchronously retrieves the order with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the order matching the specified
    /// identifier.</returns>
    /// <exception cref="NotFoundException">Thrown if no order with the specified identifier exists or the order has been deleted.</exception>
    public async Task<TbOrder> GetOrderByIdAsync(Guid id)
    {
        try
        {
            var data = await _dbContext.Set<TbOrder>()
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (data == null)
                throw new NotFoundException($"Entity of type {typeof(TbOrder).Name} with ID {id} not found.", _logger);
            return data;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting order by ID: {OrderId}", id);
            throw;
        }
    }
    public async Task<SaveResult> UpdateOrderAsync(TbOrder model, Guid updaterId, CancellationToken cancellationToken = default)
    {
        try
        {
            // ✅ Check if entity is already tracked
            var trackedEntity = _dbContext.Set<TbOrder>().Local
                .FirstOrDefault(e => e.Id == model.Id);
            if (trackedEntity != null)
            {
                // Entity is already tracked, update its properties
                _dbContext.Entry(trackedEntity).CurrentValues.SetValues(model);
            }
            else
            {
                // Entity is not tracked, attach and mark as modified
                _dbContext.Set<TbOrder>().Attach(model);
                _dbContext.Entry(model).State = EntityState.Modified;
            }
            var existingEntity = await _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == model.Id, cancellationToken);

            if (existingEntity == null)
                throw new DataAccessException($"Entity with key {model.Id} not found.", _logger);

            model.UpdatedDateUtc = DateTime.UtcNow;
            model.UpdatedBy = updaterId;
            model.CreatedBy = existingEntity.CreatedBy;
            model.IsDeleted = existingEntity.IsDeleted;
            model.CreatedDateUtc = existingEntity.CreatedDateUtc;

            var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                throw new DataAccessException($"Failed to update entity with key {model.Id}.", _logger);

            return new SaveResult { Success = true, Id = model.Id };
        }
        catch (DbUpdateConcurrencyException concurrencyEx)
        {
            throw new DataAccessException($"Concurrency error while updating an entity of type {typeof(TbOrder).Name}, ID {model.Id}.",_logger);
        }
        catch (Exception ex)
        {
            throw new DataAccessException(
                $"Error occurred while updating an entity of type {typeof(TbOrder).Name}, ID {model.Id}.", _logger);
        }
    }

    /// <summary>
    /// Check if refund exists for order detail.
    /// </summary>
    public async Task<bool> ExistsForOrderDetailAsync(Guid orderDetailId)
    {
        try
        {
            return await _dbContext.Set<TbRefund>()
                .AnyAsync(r => r.OrderDetailId == orderDetailId && !r.IsDeleted);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while checking if refund exists for order detail: {OrderDetailId}", orderDetailId);
            throw;
        }
    }

    /// <summary>
    /// Get paginated refunds with filtering.
    /// </summary>
    public async Task<(List<TbRefund> Items, int TotalCount)> GetPagedAsync(RefundSearchCriteria criteria)
    {
        try
        {
            var query = _dbContext.Set<TbRefund>()
                .Where(r => !r.IsDeleted);

            // Apply filters
            if (criteria.Status.HasValue)
                query = query.Where(r => r.RefundStatus == criteria.Status.Value);

            if (!string.IsNullOrEmpty(criteria.CustomerId))
                query = query.Where(r => r.CustomerId == Guid.Parse(criteria.CustomerId));

            if (criteria.FromDate.HasValue)
                query = query.Where(r => r.CreatedDateUtc >= criteria.FromDate.Value);

            if (criteria.ToDate.HasValue)
                query = query.Where(r => r.CreatedDateUtc <= criteria.ToDate.Value);

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination and ordering
            var items = await query
                .OrderByDescending(r => r.CreatedDateUtc)
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while getting paged refunds");
            throw;
        }
    }

    /// <summary>
    /// Create a new refund request.
    /// </summary>
    public async Task<SaveResult> CreateAsync(TbRefund refund, Guid userId)
    {
        try
        {
            var id = Guid.NewGuid();

            refund.Id = id;
            refund.CreatedDateUtc = DateTime.UtcNow;
            refund.CreatedBy = userId;
            refund.IsDeleted = false;

            await _dbContext.Set<TbRefund>().AddAsync(refund);
            await _dbContext.SaveChangesAsync();

            return new SaveResult { Success = true, Id = id };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while creating refund");
            throw;
        }
    }

    /// <summary>
    /// Update an existing refund.
    /// </summary>
    public async Task UpdateAsync(TbRefund refund, Guid userId)
    {
        try
        {
            refund.UpdatedDateUtc = DateTime.UtcNow;
            refund.UpdatedBy = userId;

            _dbContext.Set<TbRefund>().Update(refund);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while updating refund: {RefundId}", refund.Id);
            throw;
        }
    }

    /// <summary>
    /// Create refund status history record.
    /// </summary>
    public async Task<SaveResult> CreateStatusHistoryAsync(TbRefundStatusHistory historyRecord, Guid userId)
    {
        try
        {
            var id = Guid.NewGuid();

            historyRecord.Id = id;
            historyRecord.CreatedDateUtc = DateTime.UtcNow;
            historyRecord.CreatedBy = userId;

            await _dbContext.Set<TbRefundStatusHistory>().AddAsync(historyRecord);
            await _dbContext.SaveChangesAsync();

            return new SaveResult { Success = true, Id = id };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while creating refund status history");
            throw;
        }
    }

}